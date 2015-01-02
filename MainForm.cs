using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Herring
{
    public partial class MainForm : Form
    {
        private Monitor monitor;
        private Dictionary<string, int> iconIndices = new Dictionary<string, int>();
        private Font boldFont;
        private Chart chart = new Chart();

        public MainForm()
        {
            InitializeComponent();

            activitiesListView.SmallImageList = summaryListView.SmallImageList = new ImageList();            
            mainTabControl.SelectedIndex = 0;
            
            // Start the timer at a moment that guarantees maximum margin from the time unit boundaries, like this:
            //  --|--.----.----.--|--.----.----.--|--.----.----.--|--->
            // where:
            //   "-" is an interval, the time between samples are taken
            //   "." is a moment when a sample is taken
            //   "|" is a boundary of a time point
            timer.Interval = 1000 * ActivityTracker.LogTimeUnit / ActivityTracker.LogSamplingRate;
            DateTime currTimePoint = ActivityTracker.GetCurrentTimePoint();
            double interval      = (double)Parameters.LogTimeUnit / Parameters.LogSamplingRate;
            double interval_half = interval / 2.0; // [s] optimum tick shift relative to time point
            double offset        = (DateTime.Now - currTimePoint).TotalSeconds;
            int    k             = (int)Math.Floor(offset / interval_half);
            double phase_half    = offset - k * interval_half;
            double required_wait = (interval_half - phase_half) + (k % 2 == 0 ? 0 : interval_half);
            System.Threading.Thread.Sleep((int)(required_wait * 1000.0));
            timer.Start();
            
            boldFont = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
            UserStatusChanged(UserStatus.Active);
            autoScrollCheckBox.Checked = Parameters.AutoScroll;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RuleManager.Load();
            RefreshRules();

            monitor = new Monitor();
            monitor.Start();
            ActivityTracker.SetCurrentActivityLog( Persistence.Load(monitor.GetApp) );
            ActivityTracker.CurrentLogExtended += this.CurrentLogExtended;
            ActivityTracker.CurrentLogChanged += this.CurrentLogChanged;
            ActivityTracker.UserStatusChanged += this.UserStatusChanged;

            CurrentLogChanged(DateTime.Now);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Parameters.TrackActivity == true)
            {
                ActivitySnapshot snapshot = monitor.GetActivitySnapshot();
                ActivityTracker.RegisterSnapshot(snapshot);
                this.RefreshStatus(snapshot);
            }
        }

        private int GetIconIndex(AppInfo app)
        {
            if (iconIndices.ContainsKey(app.Name))
            {
                return iconIndices[app.Name];
            }
            else if (app.Icon != null)
            {
                int result = iconIndices.Count;
                iconIndices.Add(app.Name, result);
                activitiesListView.SmallImageList.Images.Add(ShellIcon.ConvertIconToBitmap(app.Icon));
                return result;
            }
            else
            {
                return -1;
            }
        }

        private void AddToActivitiesList(ActivitySummary summary)
        {
            // Header
            {
                string[] content = new string[]
                {
                    /* time: */    summary.TimePoint.ToString(),
                    /* title: */   "",
                    /* subtitle: */"",
                    /* share: */   summary.TotalShare.ToString("F1"),
                    /* keyboard:*/ summary.TotalKeyboardIntensity.ToString("F1"),
                    /* mouse:*/    summary.TotalMouseIntensity.ToString("F1"),
                    /* category:*/ ""
                };

                ListViewItem header = new ListViewItem(content);
                header.BackColor = SystemColors.ActiveCaption;  // ?
                header.Font = boldFont;
                activitiesListView.Items.Add(header);
            }

            // Entries
            foreach (ActivityEntry e in summary.Entries)
            {
                if (e.Share >= Parameters.MinimumShare)
                {
                    string category = e.Category;//RuleManager.MatchCategory(e);
                    string subtitle = e.Subtitle;

                    string[] content = new string[]
                    {
                        /* process: */  e.App.Name,
                        /* title: */    e.ApplicationTitle,
                        /* subtitle: */ subtitle,
                        /* share: */    e.Share.ToString("F1"),
                        /* keyboard: */ e.KeyboardIntensity.ToString("F1"),
                        /* mouse: */    e.MouseIntensity.ToString("F1"),
                        /* category: */ category
                    };

                    int iconIndex = GetIconIndex(e.App);

                    ListViewItem item = new ListViewItem(content, iconIndex);

                    item.UseItemStyleForSubItems = false;
                    item.ForeColor =
                        e.Share >= 20.0 ? Color.Black :
                        e.Share >= 10.0 ? Color.FromArgb(64, 64, 64) :
                                          Color.FromArgb(128, 128, 128);
                    for (int i = 0; i < content.Length - 1; ++i)
                    {
                        item.SubItems[i].ForeColor = item.ForeColor;
                    }
                    item.SubItems[6].ForeColor = Chart.GetColor(e.CategoryIndex + 1);
                    activitiesListView.Items.Add(item);
                }
            }
        }

        private void MaybeScrollActivitiesList()
        {
            if (Parameters.AutoScroll == true)
            {
                int count = activitiesListView.Items.Count;
                if (count >= 1)
                {
                    activitiesListView.EnsureVisible(count - 1);
                }
            }
        }
        
        private void RefreshActivitiesList()
        {
            activitiesListView.Items.Clear();
            foreach (ActivitySummary a in ActivityTracker.SelectedLog)
            {
                AddToActivitiesList(a);
            }
            MaybeScrollActivitiesList();
        }

        private void RefreshRules()
        {
            rulesListView.Items.Clear();
            foreach (var rule in RuleManager.Rules)
            {
                string[] content = new string[]
                {
                    rule.Process,
                    rule.Title,
                    rule.KeyboardMin.ToString("F1"),
                    rule.KeyboardMax == double.PositiveInfinity ? "" : rule.KeyboardMax.ToString("F1"),
                    rule.MouseMin.ToString("F1"),
                    rule.MouseMax == double.PositiveInfinity ? "" : rule.MouseMax.ToString("F1"),
                    rule.StatusMin.ToString(),
                    rule.StatusMax.ToString(),
                    rule.Category
                };

                ListViewItem item = new ListViewItem(content);
                item.Checked = true;
                rulesListView.Items.Add(item);
            }
        }

        class CategoryStats
        {
            public double TotalTime;
            public double ShareSum;
        }

        private void RefreshCategories()
        {
            categoriesListView.Items.Clear();
            Dictionary<string, CategoryStats> stats = new Dictionary<string, CategoryStats>();
            double totalShareOfAll = 0;
            foreach (var summary in ActivityTracker.SelectedLog)
            {
                foreach (var entry in summary.Entries)
                {
                    string category = RuleManager.MatchCategory(entry);
                    if (stats.ContainsKey(category) == false)
                    {
                        stats[category] = new CategoryStats();
                    }
                    stats[category].TotalTime += Parameters.LogTimeUnit * entry.Share / 100.0;
                    stats[category].ShareSum += entry.Share;
                    totalShareOfAll += entry.Share;
                }
            }

            List<KeyValuePair<string, CategoryStats>> statsList = stats.ToList();
            statsList.Sort((a, b) => (int)(1000 * (b.Value.ShareSum - a.Value.ShareSum)));

            TimeSpan totalTime = TimeSpan.Zero;
            foreach (var skv in statsList)
            {
                string name = skv.Key;
                CategoryStats cs = skv.Value;
                TimeSpan span = TimeSpan.FromSeconds(cs.TotalTime);
                string[] content = new string[]
                {
                    name,
                    span.ToString(),
                    (100.0 * cs.ShareSum / totalShareOfAll).ToString("F1")
                };
                ListViewItem item = new ListViewItem(content);
                categoriesListView.Items.Add(item);
                
                totalTime += span;
            }

            {
                string[] content = new string[]
                {
                    "Total",
                    totalTime.ToString(),
                    "100.0"
                };
                ListViewItem item = new ListViewItem(content);
                item.Font = boldFont;
                categoriesListView.Items.Add(item);
            }
        }


        struct ActivityId
        {
            public string ProcessName;
            public string ApplicationTitle;
            public string WindowTitle;
        }

        private void RefreshSummary()
        {
            summaryListView.Items.Clear();
            Dictionary<ActivityId, ActivityDaySummary> summaryItems = new Dictionary<ActivityId, ActivityDaySummary>();
            foreach (var summary in ActivityTracker.SelectedLog)
            {
                foreach (var entry in summary.Entries)
                {
                    ActivityId id = new ActivityId
                    {
                        ProcessName = entry.App.Path,
                        ApplicationTitle = entry.ApplicationTitle,
                        WindowTitle = entry.WindowTitle
                    };

                    if (summaryItems.ContainsKey(id) == false)
                    {
                        summaryItems[id] =
                            new ActivityDaySummary
                            {
                                 App = entry.App,
                                 ApplicationTitle = entry.ApplicationTitle,
                                 WindowTitle = entry.WindowTitle,
                                 DocumentName = entry.DocumentName
                            };
                    }
                    summaryItems[id].TotalTime += Parameters.LogTimeUnit * entry.Share / 100.0;
                }
            }

            List<ActivityDaySummary> summaryList = summaryItems.Values.ToList();
            summaryList.Sort((a, b) => (int)(b.TotalTime - a.TotalTime));

            TimeSpan totalTime = TimeSpan.Zero;
            foreach (var ads in summaryList)
            {
                TimeSpan span = TimeSpan.FromSeconds(ads.TotalTime);
                string[] content = new string[]
                {
                    ads.App.Name,
                    ads.ApplicationTitle,
                    ads.Subtitle,
                    span.ToString()
                };
                ListViewItem item = new ListViewItem(content, GetIconIndex(ads.App));
                summaryListView.Items.Add(item);
                totalTime += span;
            }
        }

        private void RefreshStatus(ActivitySnapshot snapshot)
        {
            statusLabel.Text = snapshot.App.Name;
            applicationLabel.Text = snapshot.ApplicationTitle;
            if (snapshot.WindowTitle == snapshot.ApplicationTitle)
            {
                titleLabel.Text = "";
            }
            else
            {
                titleLabel.Text = snapshot.WindowTitle;
            }
            documentLabel.Text = snapshot.DocumentName;
            statsLabel.Text = "keyboard: " + snapshot.KeyboardIntensity.ToString("F2") + ", mouse: " + snapshot.MouseIntensity.ToString("F2");
        }

        private void CurrentLogExtended(ActivitySummary summary)
        {
            AddToActivitiesList(summary);
            MaybeScrollActivitiesList();
            RefreshCategories();
            RefreshSummary();
            chart.UpdateChart(summary);
        }

        private void CurrentLogChanged(DateTime date)
        {
            RefreshActivitiesList();
            MaybeScrollActivitiesList();
            datePicker.Value = date;
            RefreshCategories();
            RefreshSummary();
            RefreshChart();
        }

        private void UserStatusChanged(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Active:
                    labelUserStatus.Text = "ACTIVE";
                    labelUserStatus.ForeColor = Color.Green;
                    break;
                case UserStatus.Passive:
                    labelUserStatus.Text = "PASSIVE";
                    labelUserStatus.ForeColor = Color.Blue;
                    break;
                case UserStatus.Away:
                    labelUserStatus.Text = "AWAY";
                    labelUserStatus.ForeColor = Color.Red;
                    break;
            }
        }

        private void RefreshChart()
        {
            chart.CreateChart(ActivityTracker.SelectedLog);
            chartBox.Image = chart.Bitmap;
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            if (datePicker.Value.Date == DateTime.Now.Date)
            {
                ActivityTracker.SetCurrentActivityLog();
            }
            else
            {
                ActivityTracker.SetSelectedActivityLog(Persistence.Load(monitor.GetApp, datePicker.Value.Date));
            }
            RefreshActivitiesList();
            RefreshCategories();
            RefreshSummary();
            RefreshChart();
        }

        private void buttonPrevDay_Click(object sender, EventArgs e)
        {
            datePicker.Value = datePicker.Value.AddDays(-1);
        }

        private void buttonNextDay_Click(object sender, EventArgs e)
        {
            datePicker.Value = datePicker.Value.AddDays(+1);
        }

        private void todayButton_Click(object sender, EventArgs e)
        {
            datePicker.Value = DateTime.Now.Date;
        }

        private void autoScrollCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Parameters.AutoScroll = autoScrollCheckBox.Checked;
        }

        private void trackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Parameters.TrackActivity = trackCheckBox.Checked;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon_MouseClick(sender, e);
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();
                this.ShowInTaskbar = true;
                notifyIcon.Visible = false;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                HideToTray();
                e.Cancel = true;
            }
        }


        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            this.FormClosing -= this.MainForm_FormClosing;
            this.Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Activity tracking is about to be turned off. Are you sure?", "Herring Activity Tracker", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.FormClosing -= this.MainForm_FormClosing;
                this.Close();
            }
        }

        private void summaryListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (summaryListView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    copyFromSummaryMenuStrip.Show(Cursor.Position);
                }
            } 
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            string text = summaryListView.FocusedItem.SubItems[1].Text;
            string url = summaryListView.FocusedItem.SubItems[2].Text;

            PutItemToClipboard(text, url);
        }

        private void activitiesListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (activitiesListView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    copyFromActivitiesMenuStrip.Show(Cursor.Position);
                }
            } 
        }

        private void copyFromActivitiesItem_Click(object sender, EventArgs e)
        {
            string text = activitiesListView.FocusedItem.SubItems[1].Text;
            string url = activitiesListView.FocusedItem.SubItems[2].Text;

            PutItemToClipboard(text, url);
        }

        private static void PutItemToClipboard(string text, string url)
        {
            if (url.StartsWith("http"))
            {
                ClipboardHelper.CopyToClipboard(String.Format("<a href=\"{0}\">{1}</a>", url, text), text + "\n" + url);
            }
            else
            {
                Clipboard.SetText(text, TextDataFormat.UnicodeText);
            }
        }

        private void HideToTray()
        {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(500);
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {   
            //this.Invoke(HideToTray());
        }
    }
}

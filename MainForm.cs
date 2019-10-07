using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Herring
{
   public partial class MainForm : Form
    {

        private Monitor monitor;
        private ReportForm reportForm;
        private Dictionary<string, int> iconIndices = new Dictionary<string, int>();
        private Font boldFont;

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

            reportForm = new ReportForm(monitor.GetApp);

            List<string> errors;

            ActivityTracker.SetCurrentActivityLog( Log.Load(monitor.GetApp, DateTime.Now, out errors));

            if (errors != null && errors.Count != 0)
            {
                ReportErrorDuringLoading(errors);
            }

            ActivityTracker.CurrentLogExtended += CurrentLogExtended;
            ActivityTracker.CurrentLogChanged += CurrentLogChanged;
            ActivityTracker.UserStatusChanged += UserStatusChanged;

            CurrentLogChanged(DateTime.Now);
        }

        private void ReportErrorDuringLoading(List<string> errors)
        {
            if (errors == null || errors.Count == 0)
                return;

            errors.Insert(0, "");

            MessageBox.Show("Errors occurred during loading saved data:"
                    + Environment.NewLine + Environment.NewLine + errors.Aggregate((a, b) => a += b.Substring(0,Math.Min(30, b.Length)) + Environment.NewLine)
                    + Environment.NewLine + "All invalid entries were skipped.",

                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
           if (!Parameters.TrackActivity) 
              return;

           var snapshot = monitor.GetActivitySnapshot();
           ActivityTracker.RegisterSnapshot(snapshot);

           nextPoint.Text = ActivityTracker.GetTimePoint(snapshot.Time, 300).ToShortTimeString();

           RefreshStatus(snapshot);
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
            // Nothing to show
            if (summary.Entries.All(e => e.Share < Parameters.MinimumShare))
                return;

            // Header
            
            var contentHeader = new []
            {
                /* time: */    summary.TimePoint.ToString(CultureInfo.InvariantCulture),
                /* title: */   "",
                /* subtitle: */"",
                /* share: */   summary.TotalShare.ToString("F1"),
                /* keyboard:*/ summary.TotalKeyboardIntensity.ToString("F1"),
                /* mouse:*/    summary.TotalMouseIntensity.ToString("F1"),
                /* category:*/ ""
            };

            ListViewItem header = new ListViewItem(contentHeader);
            header.BackColor = SystemColors.ActiveCaption;  // ?
            header.Font = boldFont;
            activitiesListView.Items.Add(header);
            

            // Entries
            foreach (var entry in summary.Entries)
            {
                if (entry.Share >= Parameters.MinimumShare)
                {
                    int iconIndex = GetIconIndex(entry.App);

                    ListViewItem item = new ActionListViewItem(entry, iconIndex, summary, header); 
                    activitiesListView.Items.Add(item);
                }
            }
        }

        private void MaybeScrollActivitiesList()
        {
            if (Parameters.AutoScroll)
            {
                int count = activitiesListView.Items.Count;
                if (count >= 1)
                {
                   Helpers.Suspend(activitiesListView);
                   activitiesListView.EnsureVisible(count - 1);
                   Helpers.Resume(activitiesListView);
                }
            }
        }

        /// <summary>
        /// Refreshes list with fukk 24-hour span.
        /// </summary>
        private void RefreshActivitiesList()
        {
            RefreshActivitiesList(datePicker.Value.Date, new TimeSpan(24, 0, 0));
        }

        private void RefreshActivitiesList(DateTime time, TimeSpan timeSpan)
        {
            try
            {
                activitiesListView.BeginUpdate();
                activitiesListView.Items.Clear();
                foreach (var a in ActivityTracker.SelectedLog.Activities)
                {
                    var point = a.TimePoint;

                    if (point < time || point > (time + timeSpan))
                        continue;

                    AddToActivitiesList(a);
                }
                MaybeScrollActivitiesList();
            }
            finally
            {
                activitiesListView.EndUpdate();
            }
        }

        private string ToStringIfNonZero(double value)
        {
           if (Math.Abs(value) > double.Epsilon)
              return value.ToString("F1");

           return "";
        }

        private void RefreshRules()
        {
            rulesListView.Items.Clear();
            foreach (var rule in RuleManager.Rules)
            {
                var content = new []
                {
                    rule.Process,
                    rule.Title,
                    ToStringIfNonZero(rule.KeyboardMin),
                    double.IsPositiveInfinity(rule.KeyboardMax) ? "" : ToStringIfNonZero(rule.KeyboardMax),
                    ToStringIfNonZero(rule.MouseMin),
                    double.IsPositiveInfinity(rule.MouseMax) ? "" : ToStringIfNonZero(rule.MouseMax),
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
            public double TotalActiveTime;
            public double TotalPresentTime;
            public double ShareSum;
        }

        private void RefreshCategories()
        {
            categoriesListView.Items.Clear();
            Dictionary<string, CategoryStats> stats = new Dictionary<string, CategoryStats>();
            double totalShareOfAll = 0;
            double centerNom = 0;
            double centerDen = 0;
            foreach (var summary in ActivityTracker.SelectedLog.Activities)
            {
                int k = 0;
                foreach (var entry in summary.Entries)
                {
                    string category = RuleManager.MatchCategory(entry);
                    if (stats.ContainsKey(category) == false)
                    {
                        stats[category] = new CategoryStats();
                    }
                    stats[category].TotalActiveTime += Parameters.LogTimeUnit * entry.Share / 100.0;
                    if (k++ == 0)
                    {
                        stats[category].TotalPresentTime += Parameters.LogTimeUnit;
                    }
                    stats[category].ShareSum += entry.Share;
                    totalShareOfAll += entry.Share;
                }
                centerNom += summary.TimePoint.TimeOfDay.TotalMinutes * summary.TotalShare;
                centerDen += summary.TotalShare;
            }
            TimeSpan center = TimeSpan.FromMinutes(centerNom / (centerDen + 0.01));

            TimeSpan totalSpan;
            var list = ActivityTracker.SelectedLog.Activities;

            if (list.Count == 0)
            {
                totalSpan = TimeSpan.Zero;
            }
            else
            {
                totalSpan = list.Last().TimePoint - list.First().TimePoint;
            }

            var statsList = stats.ToList();
            statsList.Sort((a, b) => (int)(1000 * (b.Value.ShareSum - a.Value.ShareSum)));

            TimeSpan totalActiveTime = TimeSpan.Zero;
            TimeSpan totalPresentTime = TimeSpan.Zero;
            foreach (var skv in statsList)
            {
                string name = skv.Key;
                CategoryStats cs = skv.Value;
                TimeSpan activeTime = TimeSpan.FromSeconds(cs.TotalActiveTime);
                TimeSpan presentTime = TimeSpan.FromSeconds(cs.TotalPresentTime);
                string[] content = new string[]
                {
                    name,
                    activeTime.ToString(),
                    presentTime.ToString(),
                    (100.0 * cs.ShareSum / totalShareOfAll).ToString("F1")
                };
                ListViewItem item = new ListViewItem(content);
                categoriesListView.Items.Add(item);
                
                totalActiveTime += activeTime;
                totalPresentTime += presentTime;
            }

            {
                var content = new []
                {
                    "Total Time",
                    totalActiveTime.ToString(),
                    totalPresentTime.ToString(),
                    "100.0"
                };

                ListViewItem item = new ListViewItem(content);
                item.Font = boldFont;
                categoriesListView.Items.Add(item);
            }
            {
                double intensityActive = 
                    totalSpan.TotalSeconds >= 1 ? (100.0 * totalActiveTime.TotalSeconds / totalSpan.TotalSeconds) : 0;
                double intensityPresent =
                    totalSpan.TotalSeconds >= 1 ? (100.0 * totalPresentTime.TotalSeconds / totalSpan.TotalSeconds) : 0;

                var content = new []
                {
                    "Intensity",
                    intensityActive.ToString("F2"),
                    intensityPresent.ToString("F2")
                };

                var item = new ListViewItem(content);
                item.Font = boldFont;
                categoriesListView.Items.Add(item);
            }
            {
                var content = new []
                {
                    "Total Span",
                    totalSpan.ToString()
                };

                var item = new ListViewItem(content);
                item.Font = boldFont;

                categoriesListView.Items.Add(item);
            }
            {
                var content = new []
                {
                    "Intensity Center",
                    center.ToString(@"hh\:mm\:ss")
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
        }

        /// <summary>
        /// Refreshes summary with 24h time span.
        /// </summary>
        private void RefreshSummary()
        {
            RefreshSummary(datePicker.Value.Date, new TimeSpan(24, 0, 0));
        }

        private void RefreshSummary(DateTime start, TimeSpan timeSpan)
        {
            summaryListView.Items.Clear();
            Dictionary<ActivityId, ActivityDaySummary> summaryItems = new Dictionary<ActivityId, ActivityDaySummary>();
            foreach (var summary in ActivityTracker.SelectedLog.Activities)
            {
                bool isFirst = true;
                foreach (var entry in summary.Entries)
                {
                    ActivityId id = new ActivityId
                    {
                        ProcessName = entry.App.Path,
                        ApplicationTitle = entry.ApplicationTitle
                    };

                    if (summaryItems.ContainsKey(id) == false)
                    {
                        summaryItems[id] =
                            new ActivityDaySummary
                            {
                                 App = entry.App,
                                 ApplicationTitle = entry.ApplicationTitle,
                                 DocumentUrl = entry.DocumentUrl
                            };
                    }
                    summaryItems[id].TotalTime += Parameters.LogTimeUnit * entry.Share / 100.0;
                    if (isFirst)
                    {
                        summaryItems[id].TopTime += Parameters.LogTimeUnit;
                    }
                    isFirst = false;
                }
            }

            List<ActivityDaySummary> summaryList1 = summaryItems.Values.ToList();
            summaryList1.Sort((a, b) => a.App.Name.CompareTo(b.App.Name));

            List<ActivityDaySummary> summaryList2 = new List<ActivityDaySummary>();

            // merge
            // FIXME: O(n^2) within an application
            bool[] done = new bool[summaryList1.Count];
            for (int i = 0; i < summaryList1.Count; ++i)
            {
                if (done[i] == false)
                {
                    string thisProcess = summaryList1[i].App.Name;
                    string thisTitle = summaryList1[i].ApplicationTitle;
                    string thisDocumentUrl = summaryList1[i].ValidDocumentUrl;
                    string thisDocumentSite = summaryList1[i].ValidDocumentSite;

                    ActivityDaySummary newSummary = summaryList1[i].Clone();

                    for (int j = i + 1; j < summaryList1.Count; ++j)
                    {
                        if (summaryList1[j].App.Name != thisProcess)
                        {
                            break;
                        }

                       string commonTitle;
                       if (summaryList1[j].ValidDocumentSite == thisDocumentSite &&
                           ActivityTracker.AreTitlesNearlyEqual(summaryList1[j].ApplicationTitle, thisTitle, out commonTitle))
                        {
                            thisTitle = commonTitle;
                            thisDocumentUrl = ActivityTracker.CommonUrlPart(thisDocumentUrl, summaryList1[j].DocumentUrl);

                            newSummary.ApplicationTitle = commonTitle;
                            newSummary.DocumentUrl = thisDocumentUrl;
                            newSummary.TotalTime += summaryList1[j].TotalTime;
                            newSummary.TopTime += summaryList1[j].TopTime;
                            done[j] = true;
                        }
                    }
                    summaryList2.Add(newSummary);
                }
            }

            summaryList2.Sort((a, b) => (int)(a.TopTime != b.TopTime ? b.TopTime - a.TopTime : b.TotalTime - a.TotalTime));

            TimeSpan totalTime = TimeSpan.Zero;

            try
            {
                summaryListView.BeginUpdate();

                foreach (var ads in summaryList2)
                {
                    TimeSpan span = TimeSpan.FromSeconds(ads.TotalTime);
                    TimeSpan topSpan = TimeSpan.FromSeconds(ads.TopTime);
                    string[] content = new string[]
                    {
                        ads.App.Name,
                        ads.ApplicationTitle,
                        ads.ValidDocumentUrl,
                        span.ToString(),
                        topSpan.ToString()
                    };
                    ListViewItem item = new ListViewItem(content, GetIconIndex(ads.App));
                    summaryListView.Items.Add(item);
                    totalTime += span;
                }
            }
            finally
            {
                summaryListView.EndUpdate();
            }
        }

        private void RefreshStatus(ActivitySnapshot snapshot)
        {
            processLabel.Text = snapshot.App.Name;
            titleLabel.Text = snapshot.ApplicationTitle;
            if (snapshot.WindowTitle == snapshot.ApplicationTitle)
            {
                windowLabel.Text = "";
            }
            else
            {
                windowLabel.Text = snapshot.WindowTitle;
            }
            documentUrlLabel.Text = snapshot.DocumentUrl + " [" + SystemInfo.GetUrlRetrievalTime().ToString("F0") + " ms]";
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
            chart.RefreshChart(ActivityTracker.SelectedLog);
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            List<string> errors = null;

            if (datePicker.Value.Date == DateTime.Now.Date)
            {
                ActivityTracker.SetCurrentActivityLog();
            }
            else
            {                
                ActivityTracker.SetSelectedActivityLog(Log.Load(monitor.GetApp, datePicker.Value.Date, out errors));
            }

            if (errors != null && errors.Count != 0)
            {
                ReportErrorDuringLoading(errors);
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
            if (e.Button == MouseButtons.Left)
            {
                Show();
                ShowInTaskbar = true;
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
            FormClosing -= this.MainForm_FormClosing;
            Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Activity tracking is about to be turned off. Are you sure?", "Herring Activity Tracker", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                FormClosing -= MainForm_FormClosing;
                Close();
            }
        }

        private void summaryListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (summaryListView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    summaryMenuStrip.Show(Cursor.Position);
                }
            } 
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            string proc = summaryListView.FocusedItem.SubItems[0].Text;
            string text = summaryListView.FocusedItem.SubItems[1].Text;
            string url = summaryListView.FocusedItem.SubItems[2].Text;

            PutItemToClipboard(proc, text, url);
        }

        private void activitiesListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (activitiesListView.FocusedItem.Bounds.Contains(e.Location))
                {


                    activitiesMenuStrip.Show(Cursor.Position);
                }
            } 
        }

        private void copyFromActivitiesItem_Click(object sender, EventArgs e)
        {
            string proc = activitiesListView.FocusedItem.SubItems[0].Text;
            string text = activitiesListView.FocusedItem.SubItems[1].Text;
            string url = activitiesListView.FocusedItem.SubItems[2].Text;

            PutItemToClipboard(proc, text, url);
        }

        private void copyProjectKaiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string proc = activitiesListView.FocusedItem.SubItems[0].Text;
            string text = activitiesListView.FocusedItem.SubItems[1].Text;
            string url = activitiesListView.FocusedItem.SubItems[2].Text;

            if (text.StartsWith("Project Kaiser : Osobisty : "))
            {
                text = text.Substring(28);
            }
            else if (text.StartsWith("Project Kaiser : Personal : "))
            {
                text = text.Substring(28);
            }

            PutItemToClipboard(proc, text, url);
        }

        private void activitiesMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            string proc = activitiesListView.FocusedItem.SubItems[0].Text;
            string text = activitiesListView.FocusedItem.SubItems[1].Text;

            copyProjectKaiserToolStripMenuItem.Enabled = text.StartsWith("Project Kaiser");

            string url = activitiesListView.FocusedItem.SubItems[2].Text;

            followLinkToolStripMenuItem.Enabled = (proc.StartsWith("chrome") && url != "" && !url.StartsWith("("));

        }

        private void followLinkActivitiesMenuItem_Click(object sender, EventArgs e)
        {
            string proc = activitiesListView.FocusedItem.SubItems[0].Text;
            string text = activitiesListView.FocusedItem.SubItems[1].Text;
            string url = activitiesListView.FocusedItem.SubItems[2].Text;
            if (proc.StartsWith("chrome") && url != "" && !url.StartsWith("("))
            {
                System.Diagnostics.Process.Start("https://" + url);
            }
        }

        private void followLinkSummaryMenuItem_Click(object sender, EventArgs e)
        {
            string proc = summaryListView.FocusedItem.SubItems[0].Text;
            string text = summaryListView.FocusedItem.SubItems[1].Text;
            string url = summaryListView.FocusedItem.SubItems[2].Text;
            if (proc.StartsWith("chrome") && url != "" && !url.StartsWith("("))
            {
                System.Diagnostics.Process.Start("https://" + url);
            }
        }

        private static void PutItemToClipboard(string proc, string text, string url)
        {
            if (proc.StartsWith("chrome") && url != "" && !url.StartsWith("("))
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
            mainTabControl.SelectedTab = tabPage1;
            this.Hide();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {   
            //this.Invoke(HideToTray());
        }

        private void summaryListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            double totalTime = 0;
            double totalTopTime = 0;
            foreach (ListViewItem item in summaryListView.SelectedItems)
            {
                double time = TimeSpan.Parse(item.SubItems[3].Text).TotalSeconds;
                totalTime += time;
                double topTime = TimeSpan.Parse(item.SubItems[4].Text).TotalSeconds;
                totalTopTime += topTime;
            }
            TimeSpan span1 = TimeSpan.FromSeconds(totalTime);
            TimeSpan span2 = TimeSpan.FromSeconds(totalTopTime);
            timeStatusLabel.Text = span1.ToString() + " / " + span2.ToString();
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            reportForm.ShowDialog();
        }

        private void UpdateSelectionIndicator()
        {
            if (chart.SelectionStart != null)
            {
                var startTime = chart.SelectionStart.Value;
                timeLabel.Text = $@"{startTime.Hour:00}:{startTime.Minute:00} -> ";
            }
            else
                timeLabel.Text = "";

           if (chart.SelectedBar != null)
           {
              var bar = chart.SelectedBar.Span;
              timeLabel.Text += $@"{bar.Hours:00}:{bar.Minutes:00}";
           }

            var time = chart.SelectionSpan;
            if (time.HasValue && time.Value.TotalMinutes > 0)
            {
                var minutes = (int)(time.Value.TotalMinutes);
                rangeLabel.Text = $@"{minutes / 60:00}:{minutes % 60:00}";
            }
            else
                rangeLabel.Text = @"--:--";
        }

        private void chartBox_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateSelectionIndicator();

        }

        private void chart_selectionChanged(object sender, EventArgs args)
        {
            UpdateSelectionIndicator();
        }

        private void chart_MouseUp(object sender, MouseEventArgs e)
        {
            if (chart.SelectionSpan != null && chart.SelectionStart != null)
            {
                var span = chart.SelectionSpan.GetValueOrDefault();
                var time = chart.SelectionStart.GetValueOrDefault();

                textBox1.Text = time.ToShortTimeString() + " -> " + (time + span).ToShortTimeString();

                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (chart.SelectedBar == null)
                    return;

                DateTime point = datePicker.Value.Date;
                point = point.AddMinutes(chart.SelectedBar.Minutes);

                mainTabControl.SelectTab(1);

                try
                {
                    Helpers.Suspend(activitiesListView.Parent);
                    activitiesListView.BeginUpdate(); // Clear all selection
                    activitiesListView.ClearSelection();

                    foreach (var a in activitiesListView.Items.OfType<ActionListViewItem>())
                    {
                        var span = a.Summary.TimePoint - point;

                        // Find first element whithin +/- 10 minutes
                        if (Math.Abs(span.TotalMinutes) < 10)
                        {
                            a.SelectHeader();
                            return;
                        }
                    }

                }
                finally
                {
                    activitiesListView.EndUpdate();
                    Helpers.Resume(activitiesListView.Parent);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chart.SelectionSpan != null && chart.SelectionStart != null)
            {
                var span = chart.SelectionSpan.GetValueOrDefault();
                var time = chart.SelectionStart.GetValueOrDefault() - new DateTime();

                RefreshActivitiesList(datePicker.Value.Date+time, span);
            }
        }

        private void markWorkingHoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chart.SelectionSpan != null && chart.SelectionStart != null)
            {

                DateTime date = datePicker.Value.Date;

                date = date.AddHours(chart.SelectionStart.Value.Hour);
                date = date.AddMinutes(chart.SelectionStart.Value.Minute);

                ActivityTracker.SelectedLog.MarkWorkingHours(date, chart.SelectionSpan.Value);

                RefreshChart();                
            }
        }

        private void addEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chart.SelectionSpan != null && chart.SelectionStart != null)
            {

                DateTime date = datePicker.Value.Date;

                date = date.AddHours(chart.SelectionStart.Value.Hour);
                date = date.AddMinutes(chart.SelectionStart.Value.Minute);

                using (var input = new InputBox())
                {
                    if (input.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(input.InputText))
                    {
                        ActivityTracker.SelectedLog.AddAwayFromComputer(date, chart.SelectionSpan.Value, input.InputText);
                    }
                }

                RefreshChart();
            }
        }

        private void chartMenu_Opening(object sender, CancelEventArgs e)
        {
            var selectionPresent = chart.SelectionSpan != null && chart.SelectionStart != null;

            addEventToolStripMenuItem.Enabled = selectionPresent;
            markWorkingHoursToolStripMenuItem.Enabled = selectionPresent;
            removeEventToolStripMenuItem.DropDownItems.Clear();
            removeEventToolStripMenuItem.Enabled = false;

            if (chart.SelectedBar != null)
            {
                return;
            }

            var date = datePicker.Value.Date + chart.SelectedBar.Span;

            foreach (var ev in ActivityTracker.SelectedLog.Events.Where( evnt => evnt.ContainsTimePoint(date) ))
            {
                var start = ev.Start.ToShortTimeString();
                var end =  (ev.Start + ev.Span).ToShortTimeString();

                var item = new ToolStripMenuItem( start + "->" + end + " : " +  ev.Description ?? "Unknown");
                item.Tag = ev;

                item.Click += Item_Click;

                removeEventToolStripMenuItem.DropDownItems.Add(item);
                removeEventToolStripMenuItem.Enabled = true;
            }

        }

        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Log.Event evnt = (Log.Event)item.Tag;

            ActivityTracker.SelectedLog.RemoveEvent(evnt);
            RefreshChart();
        }
    }
}

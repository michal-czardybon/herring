using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Herring
{
    public partial class MainForm : Form
    {
        private Monitor monitor;
        private Dictionary<string, int> iconIndices = new Dictionary<string, int>();
        private Font boldFont;

        public MainForm()
        {
            InitializeComponent();
            activitiesListView.SmallImageList = new ImageList();
            mainTabControl.SelectedIndex = 1;
            timer.Interval = 1000 * ActivityTracker.LogTimeUnit / ActivityTracker.LogSamplingRate;
            boldFont = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            monitor = new Monitor();
            monitor.Start();
            ActivityTracker.SetCurrentActivityLog( Persistence.Load(monitor.GetApp) );
            ActivityTracker.ActivitySummaryCreated += this.ActivitySummaryCreated;
            RefreshActivitiesList();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ActivitySnapshot snapshot = monitor.GetActivitySnapshot();
            ActivityTracker.RegisterSnapshot(snapshot);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date == DateTime.Now.Date)
            {
                ActivityTracker.SetCurrentActivityLog();
            }
            else
            {
                ActivityTracker.SetSelectedActivityLog(Persistence.Load(monitor.GetApp, dateTimePicker1.Value.Date));
            }
            RefreshActivitiesList();
        }

        private void AddToActivitiesList(ActivitySummary summary)
        {
            // Header
            {
                string[] content = new string[]
                {
                    /* time: */    summary.TimePoint.ToString(),
                    /* title: */   "",
                    /* share: */   summary.Entries.Sum(x => x.Share).ToString(),
                    /* keyboard:*/ summary.TotalKeyboardIntensity.ToString("F1"),
                    /* mouse:*/    summary.TotalMouseIntensity.ToString("F1")
                };

                ListViewItem header = new ListViewItem(content);
                header.BackColor = SystemColors.ActiveCaption;  // ?
                header.Font = boldFont;
                activitiesListView.Items.Add(header);
            }

            // Entries
            foreach (ActivityEntry e in summary.Entries)
            {
                string[] content = new string[]
                {
                    /* process: */  e.App.Name,
                    /* title: */    e.Title,
                    /* share: */    e.Share.ToString("F1"),
                    /* keyboard: */ e.KeyboardIntensity.ToString("F1"),
                    /* mouse: */    e.MouseIntensity.ToString("F1")
                };

                int iconIndex;
                if (iconIndices.ContainsKey(e.App.Name))
                {
                    iconIndex = iconIndices[e.App.Name];
                }
                else if (e.App.Icon != null)
                {
                    iconIndex = iconIndices.Count;
                    iconIndices.Add(e.App.Name, iconIndex);
                    activitiesListView.SmallImageList.Images.Add(ShellIcon.ConvertIconToBitmap(e.App.Icon));
                }
                else
                {
                    iconIndex = -1;
                }

                ListViewItem item = new ListViewItem(content, iconIndex);
                activitiesListView.Items.Add(item);
            }
        }
        
        private void RefreshActivitiesList()
        {
            activitiesListView.Items.Clear();
            foreach (ActivitySummary a in ActivityTracker.SelectedLog)
            {
                AddToActivitiesList(a);
            }
        }

        private void ActivitySummaryCreated(object sender, ActivitySummary summary)
        {
            AddToActivitiesList(summary);
        }

    }
}

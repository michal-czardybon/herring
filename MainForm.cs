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
        private List<ActivitySummary> currentLog;   // being tracked right now
        private List<ActivitySummary> selectedLog;  // being displayed
        private Persistence persistence;
        private Dictionary<string, int> iconIndices = new Dictionary<string,int>();

        private int logTimeUnit = 10;       // [s]
        private int logSamplingRate = 10;   // how many samples are taken for one time unit (should be at least 3)

        private DateTime currentTimePoint;
        private List<ActivitySnapshot> currentSnapshots = new List<ActivitySnapshot>();

        public MainForm()
        {
            InitializeComponent();
            activitiesListView.SmallImageList = new ImageList();
            mainTabControl.SelectedIndex = 1;
            timer.Interval = 1000 * logTimeUnit / logSamplingRate;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            monitor = new Monitor();
            persistence = new Persistence();
            currentLog = new List<ActivitySummary>();// Persistence.Load(monitor.GetApp);
            selectedLog = currentLog;
            RefreshActivitiesList();
            monitor.Start();
        }

        // Returns current time rounded down to appropriate time point
        private /*static*/ DateTime GetCurrentTimePoint()
        {
            return GetTimePoint(DateTime.Now, logTimeUnit);
        }

        private static DateTime GetTimePoint(DateTime time, int timeUnit)
        {
            // We assume time units up to 15 minutes
            System.Diagnostics.Debug.Assert(timeUnit >= 1 && timeUnit <= 15 * 60);

            // There must be an integer number of time points in an hour
            System.Diagnostics.Debug.Assert(3600 % timeUnit == 0);

            int actualTotalSeconds = time.Second + time.Minute * 60;
            int neededTotalSeconds = (actualTotalSeconds / timeUnit) * timeUnit;

            return new DateTime(time.Year, time.Month, time.Day, time.Hour, neededTotalSeconds / 60, neededTotalSeconds % 60);
        }

        private ActivitySummary GetActivitySummary(List<ActivitySnapshot> snapshots)
        {
            ActivitySummary summary =
                new ActivitySummary
                {
                    TimePoint = GetTimePoint(snapshots.First().Time, logTimeUnit),
                    Span = new TimeSpan(0, 0, logTimeUnit),
                    TotalKeyboardIntensity = (int)(from x in snapshots select x.KeyboardIntensity).Average(),
                    TotalMouseIntensity    = (int)(from x in snapshots select x.MouseIntensity)   .Average(),
                    Entries = new List<ActivityEntry>()
                };

            bool[] done = new bool[snapshots.Count];
            for (int i = 0; i < snapshots.Count; ++i)
            {
                if (done[i] == false)// && (snapshots[i].IsKeyboardActive || snapshots[i].IsMouseActive))
                {
                    int count = 1;
                    int sumKeyboard = snapshots[i].KeyboardIntensity;
                    int sumMouse = snapshots[i].MouseIntensity;
                    for (int j = i + 1; j < snapshots.Count; ++j)
                    {
                        if (snapshots[j].App.Name == snapshots[i].App.Name &&
                            snapshots[j].Title == snapshots[i].Title)
                        {
                            count++;
                            sumKeyboard += snapshots[j].KeyboardIntensity;
                            sumMouse += snapshots[j].MouseIntensity;
                            done[j] = true;
                        }
                    }
                    ActivityEntry newEntry =
                        new ActivityEntry
                        {
                            Share = 100 * count / snapshots.Count,
                            App = snapshots[i].App,
                            Title = snapshots[i].Title,
                            KeyboardIntensity = sumKeyboard / count,
                            MouseIntensity = sumMouse / count
                        };
                    summary.Entries.Add(newEntry);
                }
            }

            summary.Entries.Sort((a, b) => (int)(1000 * (b.Share - a.Share)));

            return summary;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ActivitySnapshot snapshot = monitor.GetActivitySnapshot();

            bool timePointChanged;
            if (currentSnapshots.Count == 0)
            {
                timePointChanged = false;
            }
            else
            {
                DateTime currTimePoint = GetTimePoint(snapshot.Time, logTimeUnit);
                DateTime prevTimePoint = GetTimePoint(currentSnapshots.Last().Time, logTimeUnit);
                timePointChanged = (currTimePoint != prevTimePoint);
            }

            if (timePointChanged)
            {
                // summarize the previous interval
                ActivitySummary summary = GetActivitySummary(currentSnapshots);
                
                currentLog.Add(summary);
                Persistence.Store(summary);

                if (currentLog == selectedLog)
                {
                    AddToActivitiesList(summary);
                }
                
                currentSnapshots.Clear();
            }
            else
            {
                // still the same interval
                currentSnapshots.Add(snapshot);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date == DateTime.Now.Date)
            {
                selectedLog = currentLog;
            }
            else
            {
                selectedLog = Persistence.Load(monitor.GetApp, dateTimePicker1.Value.Date);
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
                    /* process: */ "",
                    /* title: */   "",
                    /* share: */   (100 * summary.Entries.Count / logSamplingRate).ToString(),
                    /* keyboard:*/ summary.TotalKeyboardIntensity.ToString(),
                    /* mouse:*/    summary.TotalMouseIntensity.ToString()
                };

                ListViewItem header = new ListViewItem(content);
                header.BackColor = SystemColors.ActiveCaption;  // ?
                activitiesListView.Items.Add(header);
            }

            // Entries
            foreach (ActivityEntry e in summary.Entries)
            {
                string[] content = new string[]
                {
                    /* time: */     "",
                    /* process: */  e.App.Name,
                    /* title: */    e.Title,
                    /* share: */    e.Share.ToString(),
                    /* keyboard: */ e.KeyboardIntensity.ToString(),
                    /* mouse: */    e.MouseIntensity.ToString()
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
                    //activitiesListView.SmallImageList.Images.Add(topSample.App.Icon);
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
            foreach (ActivitySummary a in selectedLog)
            {
                AddToActivitiesList(a);
            }
        }

    }
}

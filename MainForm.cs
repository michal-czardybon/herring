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
        private List<ActivitySnapshot> currentLog;   // being tracked right now
        private List<ActivitySnapshot> selectedLog;  // being displayed
        private Persistence persistence;
        private Dictionary<string, int> iconIndices = new Dictionary<string,int>();

        private int logTimeUnit = 120;       // [s]
        private int logSamplingRate = 10;   // how many samples are taken for one time unit (should be at least 3)

        private DateTime currentTimePoint;
        private List<ActivitySnapshot> currentSamples = new List<ActivitySnapshot>();

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
            currentLog = Persistence.Load(monitor.GetApp);
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

        private ActivitySnapshot GetActivitySummary(List<ActivitySnapshot> samples)
        {
            ActivitySnapshot summary =
                new ActivitySnapshot
                {
                    Begin = GetTimePoint(samples.First().End, logTimeUnit),
                    Length = new TimeSpan(0, 0, logTimeUnit)                    
                };

            Dictionary<string, int> processPresence = new Dictionary<string, int>();
            foreach (var s in samples)
            {
                summary.MouseMoveDistance += s.MouseMoveDistance;
                summary.MouseClickCount += s.MouseClickCount;
                summary.KeyPressCount += s.KeyPressCount;

                if (processPresence.ContainsKey(s.App.Name) == false)
                {
                    processPresence[s.App.Name] = 0;
                }

                processPresence[s.App.Name] += 1;
            }

            List<KeyValuePair<string, int>> entries = new List<KeyValuePair<string, int>>();
            foreach (var x in processPresence)
            {
                entries.Add(x);
            }
            entries.Sort((a, b) => (b.Value - a.Value));

            summary.App = new AppInfo();
            summary.App.Name = entries.First().Key;

            return summary;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ActivitySnapshot snapshot = monitor.GetSnapshot();

            bool timePointChanged;
            if (currentSamples.Count == 0)
            {
                timePointChanged = false;
            }
            else
            {
                DateTime currTimePoint = GetTimePoint(snapshot.End, logTimeUnit);
                DateTime prevTimePoint = GetTimePoint(currentSamples.Last().End, logTimeUnit);
                timePointChanged = (currTimePoint != prevTimePoint);
            }


            if (timePointChanged)
            {
                // summarize the previous interval
                ActivitySnapshot summary = GetActivitySummary(currentSamples);
                
                currentLog.Add(summary);
                Persistence.Store(summary);

                if (currentLog == selectedLog)
                {
                    AddToActivitiesList(summary);
                }
                
                currentSamples.Clear();
            }
            else
            {
                // still the same interval
                currentSamples.Add(snapshot);
            }

            /*currentLog.Add(snapshot);
            Persistence.Store(snapshot);

            if (currentLog == selectedLog)
            {
                AddToActivitiesList(snapshot);
            }*/
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

        private void AddToActivitiesList(ActivitySnapshot snapshot)
        {
            string[] content = new string[]
            {
                snapshot.Begin.ToString(),
                snapshot.App.Name,
                snapshot.Title,
                snapshot.MouseSpeed.ToString(),
                snapshot.ClickingSpeed.ToString(),
                snapshot.TypingSpeed.ToString()
            };

            int iconIndex;
            if (iconIndices.ContainsKey(snapshot.App.Name))
            {
                iconIndex = iconIndices[snapshot.App.Name];
            }
            else if (snapshot.App.Icon != null)
            {
                iconIndex = iconIndices.Count;
                iconIndices.Add(snapshot.App.Name, iconIndex);
                activitiesListView.SmallImageList.Images.Add(snapshot.App.Icon);
            }
            else
            {
                iconIndex = -1;
            }

            ListViewItem item = new ListViewItem(content, iconIndex);
            activitiesListView.Items.Add(item);
        }

        private void RefreshActivitiesList()
        {
            activitiesListView.Items.Clear();
            foreach (ActivitySnapshot a in selectedLog)
            {
                AddToActivitiesList(a);
            }
        }

    }
}

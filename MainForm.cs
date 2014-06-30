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

        private int logTimeUnit = 60;       // [s]
        private int logSamplingRate = 10;   // how many samples are taken for one time unit (should be at least 3)

        private DateTime currentTimePoint;
        private List<ActivitySnapshot> currentSamples = new List<ActivitySnapshot>();

        public MainForm()
        {
            InitializeComponent();
            activitiesListView.SmallImageList = new ImageList();
            mainTabControl.SelectedIndex = 1;
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
            // We assume time units up to 15 minutes
            System.Diagnostics.Debug.Assert(logTimeUnit >= 1 && logTimeUnit <= 15*60);
            
            // There must be an integer number of time points in an hour
            System.Diagnostics.Debug.Assert(3600 % logTimeUnit == 0 );

            DateTime now = DateTime.Now;
            int actualTotalSeconds = now.Second + now.Minute * 60;
            int neededTotalSeconds = (actualTotalSeconds / logTimeUnit) * logTimeUnit;

            return new DateTime(now.Year, now.Month, now.Day, now.Hour, neededTotalSeconds / 60, neededTotalSeconds % 60);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ActivitySnapshot snapshot = monitor.GetSnapshot();
            //currentSamples.Add(snapshot);

            currentLog.Add(snapshot);
            Persistence.Store(snapshot);

            if (currentLog == selectedLog)
            {
                AddToActivitiesList(snapshot);
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

        private void AddToActivitiesList(ActivitySnapshot snapshot)
        {
            string[] content = new string[]
            {
                snapshot.Begin.ToString(),
                snapshot.App.Name,
                snapshot.Title,
                snapshot.MouseSpeed.ToString()
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

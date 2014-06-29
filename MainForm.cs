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
        private List<ActivitySnapshot> allData;
        private List<ActivitySnapshot> currData;
        private Monitor monitor;
        private Persistence persistence;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            monitor = new Monitor();
            persistence = new Persistence();
            allData = Persistence.Load(monitor.GetApp);
            monitor.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ActivitySnapshot snapshot = monitor.GetSnapshot();

            string[] items = new string[]
            {
                DateTime.Now.ToString(),
                snapshot.App.Name,
                snapshot.Title,
                snapshot.MouseSpeed.ToString()
            };
            activitiesListView.Items.Add( new ListViewItem(items) );

            //allData.Add(snapshot);
            //Persistence.Store(snapshot);

            // Set textBox
            /*const int maxLength = 160;
            textBox.AppendText(snapshot.CharsTyped);
            if (textBox.Text.Length > maxLength)
            {
                textBox.Text =
                    textBox.Text.Substring(textBox.Text.Length - maxLength, maxLength);
            }

            captionLabel.Text = snapshot.Title;

            // Set stats
            typingSpeedLabel.Text = snapshot.TypingSpeed.ToString();
            clickingSpeedLabel.Text = snapshot.ClickingSpeed.ToString();
            mouseSpeedLabel.Text = snapshot.MouseSpeed.ToString();

            canvasPanel.Refresh();*/
        }

    }
}

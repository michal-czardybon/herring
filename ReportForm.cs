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
    public partial class ReportForm : Form
    {
        private const int BAR_WIDTH = 4;
        private const int BAR_HEIGHT = 100;
        private const int TOP_MARGIN = 4;

        private GetAppDelegate getApp;
        private Bitmap bitmap = new Bitmap(1, 1);

        public ReportForm(GetAppDelegate _getApp)
        {
            this.getApp = _getApp;
            InitializeComponent();
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            DateTime dateFrom = datePickerFrom.Value.Date;
            DateTime dateTo = datePickerTo.Value.Date;

            double[] shares = new double[24 * 12];   // every 5 minutes

            int count = 0;
            for (DateTime d = dateFrom; d <= dateTo; d = d.AddDays(1))
            {
                count++;

                List<ActivitySummary> summaries = Persistence.Load(getApp, d);
                foreach (var s in summaries)
                {
                    int k = (s.TimePoint.Hour * 60 + s.TimePoint.Minute) / 5;
                    shares[k] += s.TotalShare / 100.0;
                }
            }

            bitmap.Dispose();
            bitmap = new Bitmap(24 * 12 * BAR_WIDTH, BAR_HEIGHT + TOP_MARGIN + 1);

            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            for (int i = 0; i < shares.Length; ++i)
            {
                double value = shares[i] / count;
                int x1 = i * BAR_WIDTH;
                int h  = (int)(BAR_HEIGHT * value);
                int y1 = BAR_HEIGHT - h + TOP_MARGIN;

                g.FillRectangle(Brushes.Blue, x1, y1, BAR_WIDTH, h);
            }
            g.Dispose();

            chartBox1.Image = bitmap;
        }
    }
}

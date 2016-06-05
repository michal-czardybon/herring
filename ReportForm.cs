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

            double[] weekly_nom = new double[7];
            double[] weekly_den = new double[7];

            int count = 0;
            for (DateTime d = dateFrom; d <= dateTo; d = d.AddDays(1))
            {
                count++;

                double dailyTotal = 0;
                List<ActivitySummary> summaries = Persistence.Load(getApp, d);
                foreach (var s in summaries)
                {
                    int k = (s.TimePoint.Hour * 60 + s.TimePoint.Minute) / 5;
                    shares[k] += s.TotalShare / 100.0;
                    dailyTotal += s.TotalShare / 100.0 * Parameters.LogTimeUnit / 3600.0; // count hours
                }

                int q = ((int)d.DayOfWeek + 6) % 7;
                weekly_nom[q] += dailyTotal;
                weekly_den[q] += 1.0;
            }

            bitmap.Dispose();
            bitmap = new Bitmap(24 * 12 * BAR_WIDTH, BAR_HEIGHT + TOP_MARGIN + 1);

            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            for (int i = 0; i < shares.Length; ++i)
            {
                double value = shares[i] / count;
                int x1 = i * BAR_WIDTH;
                int h = (int)(BAR_HEIGHT * value);
                int y1 = BAR_HEIGHT - h + TOP_MARGIN;

                g.FillRectangle(Brushes.Blue, x1, y1, BAR_WIDTH, h);
            }
            g.Dispose();

            chartBox1.Image = bitmap;

            // Text
            reportText.Lines =
                new string[]
                {
                    String.Format("Monday:    {0:F2}", weekly_nom[0] / weekly_den[0]),
                    String.Format("Tuesday:   {0:F2}", weekly_nom[1] / weekly_den[1]),
                    String.Format("Wednesday: {0:F2}", weekly_nom[2] / weekly_den[2]),
                    String.Format("Thursday:  {0:F2}", weekly_nom[3] / weekly_den[3]),
                    String.Format("Friday:    {0:F2}", weekly_nom[4] / weekly_den[4]),
                    String.Format("Saturaday: {0:F2}", weekly_nom[5] / weekly_den[5]),
                    String.Format("Sunday:    {0:F2}", weekly_nom[6] / weekly_den[6]),
                };
        }
    }
}

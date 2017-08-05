using System;
using System.Collections.Generic;
using System.Drawing;
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

            var shares = new double[24 * 12];   // every 5 minutes

            var weekly_nom = new double[7];
            var weekly_den = new int[7];

            int count = 0;
            for (var d = dateFrom; d <= dateTo; d = d.AddDays(1))
            {
                count++;

                double dailyTotal = 0;
                List<string> errors;
                List<ActivitySummary> summaries = Persistence.Load(getApp, d, out errors);
                foreach (var s in summaries)
                {
                    int k = (s.TimePoint.Hour * 60 + s.TimePoint.Minute) / 5;
                    shares[k] += s.TotalShare / 100.0;
                    dailyTotal += s.TotalShare / 100.0 * Parameters.LogTimeUnit / 3600.0; // count hours
                }

                int q = ((int)d.DayOfWeek + 6) % 7;
                weekly_nom[q] += dailyTotal;
                weekly_den[q] += 1;
            }

            bitmap.Dispose();
            bitmap = new Bitmap(24 * 12 * BAR_WIDTH, BAR_HEIGHT + TOP_MARGIN + 1);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                for (int i = 0; i < 24; ++i)
                {
                    int x = i * BAR_WIDTH * 12;
                    g.DrawLine(Pens.DarkGray, new Point(x, 0), new Point(x, bitmap.Width));
                }

                for (int i = 0; i < shares.Length; ++i)
                {
                    double value = shares[i] / count;
                    int x1 = i * BAR_WIDTH;
                    int h = (int)(BAR_HEIGHT * value);
                    int y1 = BAR_HEIGHT - h + TOP_MARGIN;

                    g.FillRectangle(Brushes.Blue, x1, y1, BAR_WIDTH, h);
                }
            }

            chartBox1.Image = bitmap;

            var lines = new string[7];
            for (int i = 0; i < 7; ++i)
            {
                var name = Enum.GetName(typeof(DayOfWeek), i);

                if (weekly_den[i] > 0)
                {
                    lines[i] = string.Format("{1,-16}{0:F2}", weekly_nom[i] / weekly_den[i], name+":");
                }
                else
                {
                    lines[i] = string.Format("{0,-16}Not in range", name+":");
                }
            }

            reportText.Lines = lines;
        }
    }
}

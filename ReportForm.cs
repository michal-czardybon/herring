using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private Bitmap bitmap2 = new Bitmap(1, 1);

        public ReportForm(GetAppDelegate _getApp)
        {
            this.getApp = _getApp;
            InitializeComponent();

            DateTime now = DateTime.Now;
            datePickerFrom.Value = new DateTime(now.Year, now.Month, 1);
            datePickerTo.Value = DateTime.Today.AddDays(-1);
        }

        private async void calculateButton_Click(object sender, EventArgs e)
        {
            DateTime dateFrom = datePickerFrom.Value.Date;
            DateTime dateTo = datePickerTo.Value.Date;

            if (dateTo >= DateTime.Today)
            {
                dateTo = DateTime.Today.AddDays(-1);
                datePickerTo.Value = dateTo;
            }

            int totalDays = (int)(dateTo - dateFrom).TotalDays + 1;
            if (totalDays <= 0)
                return;

            calculateButton.Enabled = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = totalDays;
            progressBar1.Value = 0;
            progressBar1.Visible = true;

            var shares = new double[24 * 12];   // every 5 minutes
            var weekly_nom = new double[7];
            var weekly_den = new int[7];
            int count = 0;

            var dailyTopTime = new double[totalDays];   // active hours per day
            var dailyCenter = new double[totalDays];    // intensity center, hours of day; NaN = no data

            var progress = new Progress<int>(value => progressBar1.Value = value);

            await Task.Run(() =>
            {
                int processed = 0;
                for (var d = dateFrom; d <= dateTo; d = d.AddDays(1))
                {
                    count++;

                    double dailyTotal = 0;
                    List<string> errors;
                    var log = Log.Load(getApp, d, out errors);

                    double centerNom = 0, centerDen = 0;
                    foreach (var s in log.Activities)
                    {
                        int k = (s.TimePoint.Hour * 60 + s.TimePoint.Minute) / 5;
                        shares[k] += s.TotalShare / 100.0;
                        dailyTotal += s.TotalShare / 100.0 * Parameters.LogTimeUnit / 3600.0;
                        centerNom += s.TimePoint.TimeOfDay.TotalHours * s.TotalShare;
                        centerDen += s.TotalShare;
                    }

                    dailyTopTime[processed] = log.Activities.Count * Parameters.LogTimeUnit / 3600.0;
                    dailyCenter[processed] = centerDen > 0.01 ? centerNom / centerDen : double.NaN;

                    int q = ((int)d.DayOfWeek + 6) % 7;
                    weekly_nom[q] += dailyTotal;
                    weekly_den[q] += 1;

                    ((IProgress<int>)progress).Report(++processed);
                }
            });

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

            DrawTrendChart(dailyTopTime, dailyCenter, totalDays, dateFrom);

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

            progressBar1.Visible = false;
            calculateButton.Enabled = true;
        }

        private void DrawTrendChart(double[] dailyTopTime, double[] dailyCenter, int totalDays, DateTime dateFrom)
        {
            int trendHeight = BAR_HEIGHT * 2;
            int width = 24 * 12 * BAR_WIDTH;
            int height = trendHeight + TOP_MARGIN + 1;

            bitmap2.Dispose();
            bitmap2 = new Bitmap(width, height);

            using (var g = Graphics.FromImage(bitmap2))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Aggregate daily data into 7-day blocks
                int numWeeks = (totalDays + 6) / 7;
                var weekTopTime = new double[numWeeks];
                var weekCenter  = new double[numWeeks];
                var weekTopCount    = new int[numWeeks];
                var weekCenterCount = new int[numWeeks];

                for (int i = 0; i < totalDays; i++)
                {
                    int w = i / 7;
                    weekTopTime[w] += dailyTopTime[i];
                    weekTopCount[w]++;
                    if (!double.IsNaN(dailyCenter[i]))
                    {
                        weekCenter[w] += dailyCenter[i];
                        weekCenterCount[w]++;
                    }
                }
                for (int w = 0; w < numWeeks; w++)
                {
                    if (weekTopCount[w] > 0) weekTopTime[w] /= weekTopCount[w];
                    weekCenter[w] = weekCenterCount[w] > 0 ? weekCenter[w] / weekCenterCount[w] : double.NaN;
                }

                float weekW = (float)width / numWeeks;

                // Determine scales
                double maxTopTime = 4.0;
                double minCenter = double.MaxValue, maxCenter = double.MinValue;
                for (int w = 0; w < numWeeks; w++)
                {
                    if (weekTopTime[w] > maxTopTime) maxTopTime = weekTopTime[w];
                    if (!double.IsNaN(weekCenter[w]))
                    {
                        if (weekCenter[w] < minCenter) minCenter = weekCenter[w];
                        if (weekCenter[w] > maxCenter) maxCenter = weekCenter[w];
                    }
                }
                if (minCenter == double.MaxValue) { minCenter = 8.0; maxCenter = 18.0; }
                double centerRange = maxCenter - minCenter;
                if (centerRange < 2.0) { double pad = (2.0 - centerRange) / 2; minCenter -= pad; maxCenter += pad; centerRange = 2.0; }

                maxTopTime  *= 1.1;
                minCenter   -= centerRange * 0.05;
                maxCenter   += centerRange * 0.05;
                centerRange  = maxCenter - minCenter;

                // Y helpers
                Func<double, float> topY = v =>
                    TOP_MARGIN + trendHeight - (float)(v / maxTopTime * trendHeight);
                Func<double, float> cenY = v =>
                    TOP_MARGIN + trendHeight - (float)((v - minCenter) / centerRange * trendHeight);

                // Horizontal grid lines (every 20%)
                for (int i = 1; i < 5; i++)
                {
                    int y = TOP_MARGIN + i * trendHeight / 5;
                    g.DrawLine(Pens.LightGray, 0, y, width, y);
                }

                // Vertical month separators — label placed at the BOTTOM of the line
                var d = dateFrom;
                using (var monthPen = new Pen(Color.LightGray))
                using (var labelFont = new Font("Arial", 7))
                {
                    for (int i = 0; i < totalDays; i++, d = d.AddDays(1))
                    {
                        if (d.Day == 1)
                        {
                            float x = (float)i / 7 * weekW;
                            g.DrawLine(monthPen, x, TOP_MARGIN, x, height - 1);
                            g.DrawString(d.ToString("MMM yyyy"), labelFont, Brushes.Gray, x + 2, height - 13);
                        }
                    }
                }

                // Active time line (green)
                using (var pen = new Pen(Color.FromArgb(0, 150, 0), 2))
                {
                    for (int w = 1; w < numWeeks; w++)
                    {
                        g.DrawLine(pen,
                            (w - 1) * weekW + weekW / 2, topY(weekTopTime[w - 1]),
                            w       * weekW + weekW / 2, topY(weekTopTime[w]));
                    }
                }

                // Intensity center line (orange), gaps on weeks with no data
                using (var pen = new Pen(Color.FromArgb(210, 90, 0), 2))
                {
                    int prev = -1;
                    for (int w = 0; w < numWeeks; w++)
                    {
                        if (double.IsNaN(weekCenter[w])) { prev = -1; continue; }
                        if (prev >= 0)
                            g.DrawLine(pen,
                                prev * weekW + weekW / 2, cenY(weekCenter[prev]),
                                w    * weekW + weekW / 2, cenY(weekCenter[w]));
                        prev = w;
                    }
                }

                // Bottom border
                g.DrawLine(Pens.Black, 0, trendHeight + TOP_MARGIN, width - 1, trendHeight + TOP_MARGIN);

                // Legend at top-left, 22px line spacing so labels never overlap
                int lx = 5, ly = TOP_MARGIN + 4;
                using (var font = new Font("Arial", 8))
                {
                    using (var brush = new SolidBrush(Color.FromArgb(0, 150, 0)))
                        g.FillRectangle(brush, lx, ly + 6, 18, 3);
                    g.DrawString(
                        string.Format("Active time (max {0:F1} h/day)", maxTopTime / 1.1),
                        font, Brushes.Black, lx + 24, ly);

                    ly += 22;
                    using (var brush = new SolidBrush(Color.FromArgb(210, 90, 0)))
                        g.FillRectangle(brush, lx, ly + 6, 18, 3);
                    g.DrawString(
                        string.Format("Intensity center ({0}\u2013{1})",
                            TimeSpan.FromHours(minCenter + centerRange * 0.05).ToString(@"hh\:mm"),
                            TimeSpan.FromHours(maxCenter - centerRange * 0.05).ToString(@"hh\:mm")),
                        font, Brushes.Black, lx + 24, ly);
                }
            }

            chartBox2.Image = bitmap2;
        }
    }
}

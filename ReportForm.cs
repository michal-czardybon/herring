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
        private const int BAR_HEIGHT = 100;
        private const int TOP_MARGIN = 4;

        private GetAppDelegate getApp;
        private Bitmap bitmap     = new Bitmap(1, 1);
        private Bitmap bitmap2    = new Bitmap(1, 1);  // displayed (base + legend overlay)
        private Bitmap bitmap2Base = new Bitmap(1, 1); // chart data only, no legend

        // Stored results so charts can be re-rendered on resize without re-loading data
        private double[] reportShares;       // summed share per 5-min slot across all days
        private int      reportDayCount;
        private double[] reportWeekTopTime;  // avg active h/day per Monday-aligned week
        private double[] reportWeekCenter;   // avg intensity-center (hours) per week; NaN = no data
        private int      reportNumWeeks;
        private int      reportTotalDays;
        private int      reportWeekOffset;   // days from preceding Monday to dateFrom (0 if Monday)
        private DateTime reportDateFrom;

        // Scale info stored so the legend overlay can reference it without recomputing
        private double trendMaxTopTime;
        private double trendMinCenter;
        private double trendCenterRange;

        public ReportForm(GetAppDelegate _getApp)
        {
            this.getApp = _getApp;
            InitializeComponent();

            chartBox2.MouseMove  += chartBox2_MouseMove;
            chartBox2.MouseLeave += chartBox2_MouseLeave;

            DateTime now = DateTime.Now;
            datePickerFrom.Value = new DateTime(now.Year, now.Month, 1);
            datePickerTo.Value = DateTime.Today.AddDays(-1);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (reportShares != null)
            {
                DrawHistogramChart();
                DrawTrendChart();
            }
        }

        private async void calculateButton_Click(object sender, EventArgs e)
        {
            DateTime dateFrom = datePickerFrom.Value.Date;
            DateTime dateTo   = datePickerTo.Value.Date;

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
            progressBar1.Value   = 0;
            progressBar1.Visible = true;

            var shares    = new double[24 * 12];
            var weeklyNom = new double[7];
            var weeklyDen = new int[7];
            int count = 0;

            var dailyTopTime   = new double[totalDays];
            var dailyCenterNom = new double[totalDays];
            var dailyCenterDen = new double[totalDays];

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
                        shares[k]  += s.TotalShare / 100.0;
                        dailyTotal += s.TotalShare / 100.0 * Parameters.LogTimeUnit / 3600.0;
                        centerNom  += s.TimePoint.TimeOfDay.TotalHours * s.TotalShare;
                        centerDen  += s.TotalShare;
                    }

                    dailyTopTime[processed]   = log.Activities.Count * Parameters.LogTimeUnit / 3600.0;
                    dailyCenterNom[processed] = centerNom;
                    dailyCenterDen[processed] = centerDen;

                    int q = ((int)d.DayOfWeek + 6) % 7;
                    weeklyNom[q] += dailyTotal;
                    weeklyDen[q] += 1;

                    ((IProgress<int>)progress).Report(++processed);
                }
            });

            // Aggregate daily -> Monday-aligned weeks
            int weekOffset = ((int)dateFrom.DayOfWeek + 6) % 7;  // 0=Mon … 6=Sun
            int numWeeks   = (totalDays + weekOffset + 6) / 7;
            var weekTopTime   = new double[numWeeks];
            var weekCenter    = new double[numWeeks];
            var weekTopCount  = new int[numWeeks];
            var weekCenterNom = new double[numWeeks];
            var weekCenterDen = new double[numWeeks];

            for (int i = 0; i < totalDays; i++)
            {
                int w = (i + weekOffset) / 7;
                weekTopTime[w]   += dailyTopTime[i];
                weekTopCount[w]++;
                weekCenterNom[w] += dailyCenterNom[i];
                weekCenterDen[w] += dailyCenterDen[i];
            }
            for (int w = 0; w < numWeeks; w++)
            {
                if (weekTopCount[w] > 0) weekTopTime[w] /= weekTopCount[w];
                weekCenter[w] = weekCenterDen[w] > 0.01 ? weekCenterNom[w] / weekCenterDen[w] : double.NaN;
                if (weekTopTime[w] < 0.25) weekCenter[w] = double.NaN;  // too little data to trust center
            }

            // Store for resize re-rendering
            reportShares      = shares;
            reportDayCount    = count;
            reportWeekTopTime = weekTopTime;
            reportWeekCenter  = weekCenter;
            reportNumWeeks    = numWeeks;
            reportTotalDays   = totalDays;
            reportWeekOffset  = weekOffset;
            reportDateFrom    = dateFrom;

            DrawHistogramChart();
            DrawTrendChart();

            // Weekly text summary
            var lines = new string[7];
            for (int i = 0; i < 7; ++i)
            {
                var name = Enum.GetName(typeof(DayOfWeek), i);
                lines[i] = weeklyDen[i] > 0
                    ? string.Format("{1,-16}{0:F2}", weeklyNom[i] / weeklyDen[i], name + ":")
                    : string.Format("{0,-16}Not in range", name + ":");
            }
            reportText.Lines = lines;

            progressBar1.Visible = false;
            calculateButton.Enabled = true;
        }

        private void DrawHistogramChart()
        {
            int chartWidth = Math.Max(1, chartBox1.ClientSize.Width);
            int height     = BAR_HEIGHT + TOP_MARGIN + 1;
            float barW     = (float)chartWidth / (24 * 12);

            bitmap.Dispose();
            bitmap = new Bitmap(chartWidth, height);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                for (int i = 0; i < 24; ++i)
                {
                    float x = i * barW * 12;
                    g.DrawLine(Pens.DarkGray, x, 0, x, height);
                }

                for (int i = 0; i < reportShares.Length; ++i)
                {
                    double value = reportShares[i] / reportDayCount;
                    float  x1   = i * barW;
                    int    h    = (int)(BAR_HEIGHT * value);
                    int    y1   = BAR_HEIGHT - h + TOP_MARGIN;
                    g.FillRectangle(Brushes.Blue, x1, y1, barW, h);
                }
            }

            chartBox1.Image = bitmap;
        }

        private void DrawTrendChart()
        {
            int trendHeight = BAR_HEIGHT * 3;
            int chartWidth  = Math.Max(1, chartBox2.ClientSize.Width);
            int height      = trendHeight + TOP_MARGIN + 1;
            float weekW     = (float)chartWidth / reportNumWeeks;

            // Compute and store scales
            double maxTopTime = 4.0;
            double minCenter = double.MaxValue, maxCenter = double.MinValue;
            for (int w = 0; w < reportNumWeeks; w++)
            {
                if (reportWeekTopTime[w] > maxTopTime) maxTopTime = reportWeekTopTime[w];
                if (!double.IsNaN(reportWeekCenter[w]))
                {
                    if (reportWeekCenter[w] < minCenter) minCenter = reportWeekCenter[w];
                    if (reportWeekCenter[w] > maxCenter) maxCenter = reportWeekCenter[w];
                }
            }
            if (minCenter == double.MaxValue) { minCenter = 8.0; maxCenter = 18.0; }
            double centerRange = maxCenter - minCenter;
            if (centerRange < 2.0) { double pad = (2.0 - centerRange) / 2; minCenter -= pad; maxCenter += pad; centerRange = 2.0; }

            maxTopTime  *= 1.5;
            minCenter   -= centerRange * 0.05;
            maxCenter   += centerRange * 0.4;
            centerRange  = maxCenter - minCenter;

            trendMaxTopTime  = maxTopTime;
            trendMinCenter   = minCenter;
            trendCenterRange = centerRange;

            Func<double, float> topY = v =>
                TOP_MARGIN + trendHeight - (float)(v / maxTopTime * trendHeight);
            Func<double, float> cenY = v =>
                TOP_MARGIN + trendHeight - (float)((v - minCenter) / centerRange * trendHeight);

            // Render chart data (no legend) into bitmap2Base
            bitmap2Base.Dispose();
            bitmap2Base = new Bitmap(chartWidth, height);

            using (var g = Graphics.FromImage(bitmap2Base))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Horizontal grid lines (every 20%)
                for (int i = 1; i < 5; i++)
                {
                    int y = TOP_MARGIN + i * trendHeight / 5;
                    g.DrawLine(Pens.LightGray, 0, y, chartWidth, y);
                }

                // Vertical month separators — label at the bottom of each line
                var d = reportDateFrom;
                using (var monthPen  = new Pen(Color.LightGray))
                using (var labelFont = new Font("Arial", 7))
                {
                    for (int i = 0; i < reportTotalDays; i++, d = d.AddDays(1))
                    {
                        if (d.Day == 1 && (d.Month == 1 || d.Month == 4 || d.Month == 7 || d.Month == 10))
                        {
                            float x = (float)(i + reportWeekOffset) / 7 * weekW;
                            g.DrawLine(monthPen, x, TOP_MARGIN, x, height - 1);
                            g.DrawString(d.ToString("MMM yyyy"), labelFont, Brushes.Gray, x + 2, height - 13);
                        }
                    }
                }

                // Active time line (green)
                using (var pen = new Pen(Color.FromArgb(0, 150, 0), 2))
                {
                    for (int w = 1; w < reportNumWeeks; w++)
                        g.DrawLine(pen,
                            (w - 1) * weekW + weekW / 2, topY(reportWeekTopTime[w - 1]),
                            w       * weekW + weekW / 2, topY(reportWeekTopTime[w]));
                }

                // Intensity center line (orange), gaps on weeks with no data
                using (var pen = new Pen(Color.FromArgb(210, 90, 0), 2))
                {
                    int prev = -1;
                    for (int w = 0; w < reportNumWeeks; w++)
                    {
                        if (double.IsNaN(reportWeekCenter[w])) { prev = -1; continue; }
                        if (prev >= 0)
                            g.DrawLine(pen,
                                prev * weekW + weekW / 2, cenY(reportWeekCenter[prev]),
                                w    * weekW + weekW / 2, cenY(reportWeekCenter[w]));
                        prev = w;
                    }
                }

                // Bottom border
                g.DrawLine(Pens.Black, 0, trendHeight + TOP_MARGIN, chartWidth - 1, trendHeight + TOP_MARGIN);
            }

            OverlayTrendLegend(null);
        }

        // Composites bitmap2Base + legend (scale info or hovered-week values) into bitmap2.
        // hoveredWeek = null  → show scale range; >= 0 → show that week's actual values.
        private void OverlayTrendLegend(int? hoveredWeek)
        {
            bitmap2.Dispose();
            bitmap2 = new Bitmap(bitmap2Base);

            int trendHeight = BAR_HEIGHT * 3;
            float weekW     = (float)bitmap2.Width / reportNumWeeks;

            using (var g = Graphics.FromImage(bitmap2))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (hoveredWeek.HasValue)
                {
                    int w = hoveredWeek.Value;

                    // Vertical cursor line
                    float cx = w * weekW + weekW / 2;
                    using (var cursorPen = new Pen(Color.FromArgb(160, 160, 160), 1))
                        g.DrawLine(cursorPen, cx, TOP_MARGIN, cx, trendHeight + TOP_MARGIN);

                    // Week date range
                    DateTime weekMon = reportDateFrom.AddDays(w * 7 - reportWeekOffset);
                    DateTime weekSun = weekMon.AddDays(6);

                    string topText = double.IsNaN(reportWeekCenter[w])
                        ? "no data"
                        : TimeSpan.FromHours(reportWeekCenter[w]).ToString(@"hh\:mm");

                    DrawLegend(g,
                        string.Format("{0:d MMM} \u2013 {1:d MMM yyyy}", weekMon, weekSun),
                        string.Format("Active time: {0:F1} h/day", reportWeekTopTime[w]),
                        string.Format("Intensity center: {0}", topText));
                }
                else
                {
                    DrawLegend(g,
                        null,
                        string.Format("Active time (max {0:F1} h/day)", trendMaxTopTime / 1.5),
                        string.Format("Intensity center ({0}\u2013{1})",
                            TimeSpan.FromHours(trendMinCenter + trendCenterRange * 0.05).ToString(@"hh\:mm"),
                            TimeSpan.FromHours(trendMaxTopTime - trendCenterRange * 0.05).ToString(@"hh\:mm")));
                }
            }

            chartBox2.Image = bitmap2;
        }

        private void DrawLegend(Graphics g, string header, string topTimeLine, string centerLine)
        {
            int lx = 5, ly = TOP_MARGIN + 4;
            using (var font     = new Font("Arial", 8))
            using (var boldFont = new Font("Arial", 8, FontStyle.Bold))
            {
                if (header != null)
                {
                    g.DrawString(header, boldFont, Brushes.Black, lx, ly);
                    ly += 18;
                }

                using (var brush = new SolidBrush(Color.FromArgb(0, 150, 0)))
                    g.FillRectangle(brush, lx, ly + 6, 18, 3);
                g.DrawString(topTimeLine, font, Brushes.Black, lx + 24, ly);

                ly += 22;
                using (var brush = new SolidBrush(Color.FromArgb(210, 90, 0)))
                    g.FillRectangle(brush, lx, ly + 6, 18, 3);
                g.DrawString(centerLine, font, Brushes.Black, lx + 24, ly);
            }
        }

        private void chartBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (reportWeekTopTime == null) return;
            float weekW = (float)chartBox2.ClientSize.Width / reportNumWeeks;
            int w = Math.Min((int)(e.X / weekW), reportNumWeeks - 1);
            OverlayTrendLegend(w);
        }

        private void chartBox2_MouseLeave(object sender, EventArgs e)
        {
            if (reportWeekTopTime == null) return;
            OverlayTrendLegend(null);
        }
    }
}

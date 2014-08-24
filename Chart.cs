using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Herring
{
    public class Chart
    {
        private const int BAR_WIDTH = 4;
        private const int BAR_HEIGHT = 40;
        private const int TOP_MARGIN = 2;

        private Bitmap bitmap = new Bitmap(1, 1);

        private Brush[] palette = new Brush[]
        {
            Brushes.Silver,
            Brushes.Blue,
            Brushes.Red,
            Brushes.Olive,
            Brushes.Green,
            Brushes.Violet,
            Brushes.Yellow,
            Brushes.Tomato
        };

        public Bitmap Bitmap
        {
            get
            {
                return bitmap;
            }
        }

        private void DrawSummary(Graphics g, ActivitySummary s)
        {
            DateTime time = s.TimePoint;
            int index = (int)(time - s.TimePoint.Date).TotalSeconds / Parameters.LogTimeUnit;
            double[] share = new double[RuleManager.Categories.Count + 1];
            foreach (ActivityEntry e in s.Entries)
            {
                string category = RuleManager.MatchCategory(e);
                int k = RuleManager.Categories.IndexOf(category) + 1;
                share[k] += e.Share;
            }

            float y1 = 0;
            double total = 0;
            for (int i = 0; i < share.Length; ++i)
            {
                Brush brush = palette[i % palette.Length];
                int x = index * BAR_WIDTH;
                total += share[i];
                float y2 = BAR_HEIGHT * (float)total / 100;
                g.FillRectangle(brush, index * BAR_WIDTH, TOP_MARGIN + (BAR_HEIGHT - y2), BAR_WIDTH - 1, y2 - y1);
                y1 = y2;
            }
        }

        public void CreateChart(List<ActivitySummary> log)
        {
            bitmap.Dispose();
            bitmap = new Bitmap(24 * 12 * BAR_WIDTH, BAR_HEIGHT + TOP_MARGIN + 1);

            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            foreach (ActivitySummary s in log)
            {
                DrawSummary(g, s);
            }
            g.DrawLine(Pens.Black, 0, BAR_HEIGHT + TOP_MARGIN, bitmap.Width - 1, BAR_HEIGHT + TOP_MARGIN);

            for (int i = 1; i <= 24; ++i)
            {
                int x = i * BAR_WIDTH * 12 - 1;
                Pen pen = (i % 8 == 0 ? Pens.Black : Pens.Gray);
                g.DrawLine(pen, x, 0, x, bitmap.Height);
            }
            g.Dispose();
        }

        public void UpdateChart(ActivitySummary summary)
        {
            Graphics g = Graphics.FromImage(bitmap);
            DrawSummary(g, summary);
            g.Dispose();
        }

    }
}

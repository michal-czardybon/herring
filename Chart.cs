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

        static private Color[] palette = new Color[]
        {
            Color.FromArgb(192, 192, 192),
            Color.FromArgb(31, 119, 180),
            Color.FromArgb(255, 127, 14),
            Color.FromArgb(44, 160, 44),
            Color.FromArgb(214, 39, 40),
            Color.FromArgb(148, 103, 189),
            Color.FromArgb(140, 86, 75),
            Color.FromArgb(227, 119, 194),
            Color.FromArgb(127, 127, 127),
            Color.FromArgb(160, 161, 25),
            Color.FromArgb(23, 190, 207)
        };

        static private Brush[] brushes;

        static Chart()
        {
            brushes = new SolidBrush[palette.Length];
            for (int i = 0; i < brushes.Length; ++i)
            {
                brushes[i] = new SolidBrush(palette[i]);
            }
        }

        static public Color GetColor(int k)
        {
            return palette[k % palette.Length];
        }

        static public Brush GetBrush(int k)
        {
            return brushes[k % brushes.Length];
        }

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
                int k = e.CategoryIndex + 1;
                share[k] += e.Share;
            }

            float y1 = 0;
            double total = 0;
            for (int i = 0; i < share.Length; ++i)            
            {
                Brush brush = GetBrush(i);
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
                Pen pen = (i % 8 == 0 ? Pens.Black : Pens.LightGray);
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

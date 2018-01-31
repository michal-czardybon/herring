using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Herring
{
    public class Chart
    {
        private const int BAR_WIDTH = 4;
        private const int BAR_HEIGHT = 40;
        private const int TOP_MARGIN = 4;

        private Bitmap bitmap = new Bitmap(1, 1);

        private static readonly Color[] Palette = 
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
            Color.FromArgb(140, 141, 25),
            Color.FromArgb(23, 190, 207),
            Color.FromArgb(80, 130, 120),
            Color.FromArgb(74, 120, 0),
            Color.FromArgb(100, 70, 70),
        };

        static private Brush[] brushes;

        static Chart()
        {
            brushes = new SolidBrush[Palette.Length];
            for (int i = 0; i < brushes.Length; ++i)
            {
                brushes[i] = new SolidBrush(Palette[i]);
            }
        }

        static public Color GetColor(int k)
        {
            return Palette[k % Palette.Length];
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

        public void CreateChart(Log log, int? selectedBar, int rangeStart, int rangeEnd)
        {
            bitmap.Dispose();
            bitmap = new Bitmap(24 * 12 * BAR_WIDTH, BAR_HEIGHT + TOP_MARGIN + 1);

            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            if (rangeEnd != -1 && rangeStart != -1)
            {
                int start = (rangeStart / BAR_WIDTH) * BAR_WIDTH;
                int end = (rangeEnd / BAR_WIDTH) * BAR_WIDTH;

                g.FillRectangle(SystemBrushes.Highlight, new Rectangle(start, 0, end - start, bitmap.Height));
            }

            using (var brush = new SolidBrush(Color.FromArgb(60, 0, 100, 200)))
            {
                foreach (var e in log.Events.Where(ev => ev.Type == Log.EventType.AwayFromComputer))
                {
                    var begin = e.Start.Hour * 60 + e.Start.Minute;
                    var end = (int)(e.Span.TotalHours * 60);

                    g.FillRectangle(brush, 
                        (begin / 5) * 4, 
                        0, 
                        (end / 5) * 4, 
                        bitmap.Height);

                }
            }

            foreach (ActivitySummary s in log.Activities)
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

            using (var pen = new Pen(Color.Red, 2))
            {
                foreach (var e in log.Events.Where(ev => ev.Type == Log.EventType.WorkignHours))
                {
                    var begin = e.Start.Hour * 60 + e.Start.Minute;
                    var end = begin + (int)(e.Span.TotalHours * 60);

                    g.DrawLine(pen, (begin / 5) * 4, bitmap.Height - 2, (end / 5) * 4, bitmap.Height - 2);

                }
            }

            if (selectedBar != null)
            {
                int x = selectedBar.Value  * BAR_WIDTH - 1;

                g.DrawLine(Pens.Red, x, 0, x, bitmap.Height);
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace Herring
{
    public class Chart
    {
        private const int BAR_WIDTH = 5;
        private const int BAR_HEIGHT = 40;
        private const int TOP_MARGIN = 2;

        private Bitmap bitmap = new Bitmap(1, 1);

        public Bitmap Bitmap
        {
            get
            {
                return bitmap;
            }
        }

        public void CreateChart(List<ActivitySummary> log)
        {
            if (log.Count >= 1)
            {
                DateTime start = log[0].TimePoint;
                DateTime end = log[log.Count - 1].TimePoint;
                int length = ((int)((end - start).TotalSeconds) / Parameters.LogTimeUnit + 1);

                bitmap.Dispose();
                bitmap = new Bitmap(length * BAR_WIDTH, BAR_HEIGHT + TOP_MARGIN + 1);

                Graphics g = Graphics.FromImage(bitmap);
                g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

                foreach (ActivitySummary s in log)
                {
                    DateTime time = s.TimePoint;
                    int index = (int)(time - start).TotalSeconds / Parameters.LogTimeUnit;
                    int x1 = index * BAR_WIDTH;
                    int y1 = BAR_HEIGHT;

                    int h = (int)(BAR_HEIGHT * s.TotalShare / 100);
                    g.FillRectangle(Brushes.Silver, x1, TOP_MARGIN + (BAR_HEIGHT - h), BAR_WIDTH - 1, h);
                }
                g.DrawLine(Pens.Black, 0, BAR_HEIGHT + TOP_MARGIN, bitmap.Width - 1, BAR_HEIGHT + TOP_MARGIN);
                g.Dispose();

                bitmap.Save("d:\\output.png");
            }
            else
            {
                bitmap.Dispose();
                bitmap = new Bitmap(1, 1);
            }
        }

    }
}

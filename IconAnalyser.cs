using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Herring
{
    public class ColorBin
    {
        private double r;
        private double g;
        private double b;
        private double size;

        public double Size
        {
            get { return size; }
        }

        public Color ToColor()
        {
            return Color.FromArgb((int)(r / size), (int)(g / size), (int)(b / size));
        }

        public void Add(double r, double g, double b, double a)
        {
            this.r += r * a;
            this.g += g * a;
            this.b += b * a;
            this.size += a;
        }

        public bool Matches(double r, double g, double b)
        {
            const double limit = 0.4;

            double v1 = (this.r + this.g + this.b) / size / 3;
            double r1 = this.r / size / v1;
            double g1 = this.g / size / v1;
            double b1 = this.b / size / v1;

            double v2 = (r + g + b) / 3;
            double r2 = r / v2;
            double g2 = g / v2;
            double b2 = b / v2;

            double dr = r1 - r2;
            double dg = g1 - g2;
            double db = b1 - b2;
            
            return (dr * dr + dg * dg + db * db) < limit * limit;
        }

        public void Normalize(double total)
        {
            r /= total;
            g /= total;
            b /= total;
            size /= total;
        }
    }


    class IconAnalyser
    {
        public static ColorBin[] GetColors(Icon icon)
        {
            if (icon == null)
            {
                ColorBin[] result = new ColorBin[1];
                result[0] = new ColorBin();
                result[0].Add(255, 0, 0, 1);
                result[0].Normalize(1);
                return result;
            }

            Bitmap bmp = icon.ToBitmap();
            System.Diagnostics.Debug.Assert(bmp.PixelFormat == PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            List<ColorBin> bins = new List<ColorBin>();
            for (int i = 0; i < bmpData.Height; ++i)
            {
                unsafe
                {
                    byte* ptr = ((byte*)bmpData.Scan0.ToPointer()) + bmpData.Stride * i;
                    for (int j = 0; j < bmpData.Width; ++j)
                    {
                        if (ptr[3] >= 1)
                        {
                            double a = ptr[3];
                            double r = ptr[2];
                            double g = ptr[1];
                            double b = ptr[0];
                            if (r + g + b >= 1.0)
                            {
                                bool found = false;
                                for (int k = 0; k < bins.Count; ++k)
                                {
                                    if (bins[k].Matches(r, g, b))
                                    {
                                        bins[k].Add(r, g, b, a);
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    ColorBin newBin = new ColorBin();
                                    newBin.Add(r, g, b, a);
                                    bins.Add(newBin);
                                }
                            }
                        }
                        ptr += 4;
                    }
                }
            }
            bmp.UnlockBits(bmpData);

            // Normalize the bins
            double sum = 0;
            for (int k = 0; k < bins.Count; ++k)
            {
                sum += bins[k].Size;
            }
            for (int k = 0; k < bins.Count; ++k)
            {
                bins[k].Normalize(sum);
            }

            return bins.ToArray();
        }

        public static Color GetAverageColor(Icon icon)
        {
            Bitmap bmp = icon.ToBitmap();
            System.Diagnostics.Debug.Assert(bmp.PixelFormat == PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            double r = 0;
            double g = 0;
            double b = 0;
            double n = 0;
            for (int i = 0; i < bmpData.Height; ++i)
            {
                unsafe
                {
                    byte* ptr = ((byte*)bmpData.Scan0.ToPointer()) + bmpData.Stride * i;
                    for (int j = 0; j < bmpData.Width; ++j)
                    {
                        double a = (double)ptr[3];
                        r += a * ptr[2];
                        g += a * ptr[1];
                        b += a * ptr[0];
                        n += a;
                        ptr += 4;
                    }
                }
            }
            bmp.UnlockBits(bmpData);
            if (n >= 1)
                return Color.FromArgb((int)(r / n), (int)(g / n), (int)(b / n));
            else
                return Color.Black;
        }
    }
}

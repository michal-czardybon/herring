using System;
using System.Collections.Generic;
using System.Text;
using gma.System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Herring
{
    delegate AppInfo GetAppDelegate(string path);

    class Monitor
    {
        private UserActivityHook actHook;
        private Dictionary<string, AppInfo> apps = new Dictionary<string, AppInfo>();

        private DateTime begin;
        private StringBuilder charsTyped;
        private double mouseDistance;
        private int mouseClicks;
        private Point prevMouseLoc;

        public Monitor()
        {
            Reset(DateTime.Now);

            actHook = new UserActivityHook();
            actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            actHook.KeyDown         += new KeyEventHandler(KeyDown);
            actHook.KeyPress += new KeyPressEventHandler(KeyPressed);
        }

        public void Start()
        {
            actHook.Start();
        }

        public void MouseMoved(object sender, MouseEventArgs e)
        {
            // Distance
            if (prevMouseLoc.X != -1)
            {
                double dx = e.Location.X - prevMouseLoc.X;
                double dy = e.Location.Y - prevMouseLoc.Y;
                double d = Math.Sqrt(dx * dx + dy * dy);
                mouseDistance += d;
            }
            prevMouseLoc = e.Location;

            // Clicks
            mouseClicks += e.Clicks / 2;
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {           
        }

        public void KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < ' ')        // \t, \r, \n etc.
            {
                if (e.KeyChar == '\b')  // backspace
                {
                    if (charsTyped.Length >= 1)
                        charsTyped.Remove(charsTyped.Length - 1, 1);
                }
                else
                {
                    charsTyped.Append(' ');
                }
            }
            else
            {
                charsTyped.Append(e.KeyChar);
            }
        }

        // Takes "now" for strictly precise measurements
        public void Reset(DateTime now)
        {
            begin = now;
            charsTyped = new StringBuilder();
            prevMouseLoc = new Point(-1, -1);
            mouseDistance = 0;
            mouseClicks = 0;
        }

        public AppInfo GetApp(string path)
        {
            AppInfo appInfo;
            if (apps.ContainsKey(path))
            {
                appInfo = apps[path];
            }
            else
            {
                Icon icon = ShellIcon.GetLargeIcon(path);
                appInfo =
                    new AppInfo
                    {
                        Name = Path.GetFileName(path),
                        Path = path,
                        Icon = icon,
                        ColorBins = IconAnalyser.GetColors(icon)
                    };
                apps.Add(path, appInfo);
            }
            return appInfo;
        }

        public ActivitySnapshot GetSnapshot()
        {
            string path = SystemInfo.GetTopWindowPath();
            //string name = SystemInfo.GetTopWindowName();
            string text = SystemInfo.GetTopWindowText();

            // Prepare AppInfo object
            AppInfo appInfo = GetApp(path);

            // Measure the time span since the previous snapshot
            DateTime end = DateTime.Now;
            TimeSpan length = end - begin;

            // Create an activity data
            ActivitySnapshot data =
                new ActivitySnapshot
                {
                    Begin = begin,
                    Length = DateTime.Now - begin,
                    App = appInfo,
                    Title = text,
                    CharsTyped = charsTyped.ToString(),
                    MouseClicks = mouseClicks,
                    MouseDistance = mouseDistance
                };

            Reset(end);

            return data;
        }

    }
}

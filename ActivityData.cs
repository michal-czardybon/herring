using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Herring
{
    /// Information about an application installed in the system
    class AppInfo
    {
        public string       Name;
        public string       Path;
        public Icon         Icon;
        public ColorBin[]   ColorBins;
    }

    /// Basic unit of information stored in the program
    class ActivitySnapshot
    {
        public DateTime Begin;
        public TimeSpan Length;
        public AppInfo  App;
        public string   Title;
        public string   CharsTyped;
        public double   MouseDistance;
        public int      MouseClicks;
        //public double   CpuLoad;

        public int TypingSpeed
        {
            get
            {
                double words = CharsTyped.Length / 5.0;
                double minutes = Length.TotalMilliseconds / 60000.0;
                return (int)(words / minutes + 0.5);
            }
        }

        public int ClickingSpeed
        {
            get
            {
                double clicks = MouseClicks;
                double minutes = Length.TotalMilliseconds / 60000.0;
                return (int)(clicks / minutes + 0.5);
            }
        }

        public int MouseSpeed
        {
            get
            {
                double distance = MouseDistance;
                double seconds = Length.TotalMilliseconds / 1000.0;
                return (int)(distance / seconds + 0.5);
            }
        }


    }

}

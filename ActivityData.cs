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
        public AppInfo App;
        public string  Title;
        public int KeyPressCount;
        public int MouseClickCount;
        public double MouseMoveDistance;
        //public string TypedText;
        //public double   CpuLoad;

        public int TypingSpeed
        {
            get
            {
                double chars = KeyPressCount;
                double minutes = Length.TotalMilliseconds / 60000.0;
                return (int)(chars / minutes + 0.5);
            }
        }

        public int ClickingSpeed
        {
            get
            {
                double clicks = MouseClickCount;
                double minutes = Length.TotalMilliseconds / 60000.0;
                return (int)(clicks / minutes + 0.5);
            }
        }

        public int MouseSpeed
        {
            get
            {
                double distance = MouseMoveDistance;
                double seconds = Length.TotalMilliseconds / 1000.0;
                return (int)(distance / seconds + 0.5);
            }
        }


    }

}

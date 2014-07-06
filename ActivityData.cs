using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

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

    /// The basic piece of information about user activity
    class ActivitySample
    {
        public AppInfo  App;
        public string   Title;
        public int      KeyboardIntensity;
        public int      MouseIntensity;

        public bool IsKeyboardActive
        {
            get { return KeyboardIntensity >= 1; }    // one word per minute
        }

        public bool IsMouseActive
        {
            get { return MouseIntensity >= 1; }       // 1000 pixels per minute
        }

    }

    // Information gathered at a single time moment
    class ActivitySnapshot : ActivitySample
    {
        public DateTime Time;   // This is the END of the time-span        

        public static int GetIntensity(int rawValue, int unit, TimeSpan span)
        {
            double minutes = span.TotalMilliseconds / 60000.0;
            double value = (double)rawValue / unit;
            return (int)(value / minutes);
        }
    }

    class ActivityEntry : ActivitySample
    {
        public int Share;
    }
        
    // Information got from many time quantuums
    class ActivitySummary
    {
        public DateTime TimePoint;  // This is the START of the time span
        public TimeSpan Span;
        public List<ActivityEntry> Entries;
        
        public int TotalShare;
        public int TotalKeyboardIntensity;
        public int TotalMouseIntensity;
    }

}

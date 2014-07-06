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
        public double   KeyboardIntensity;
        public double   MouseIntensity;

        public bool IsKeyboardActive
        {
            get { return KeyboardIntensity >= 0.5; }    // one word per minute
        }

        public bool IsMouseActive
        {
            get { return MouseIntensity >= 0.5; }       // 1000 pixels per minute
        }

        public bool IsActive
        {
            get { return IsKeyboardActive || IsMouseActive; }
        }

    }

    // Information gathered at a single time moment
    class ActivitySnapshot : ActivitySample
    {
        public DateTime Time;   // This is the END of the time-span        

        public static double GetIntensity(double rawValue, int unit, TimeSpan span)
        {
            double minutes = span.TotalMilliseconds / 60000.0;
            double value = rawValue / unit;
            return value / minutes;
        }
    }

    class ActivityEntry : ActivitySample
    {
        public double Share;
    }
        
    // Information got from many time quantuums
    class ActivitySummary
    {
        public DateTime TimePoint;  // This is the START of the time span
        public TimeSpan Span;
        public List<ActivityEntry> Entries;
        
        public double TotalShare;
        public double TotalKeyboardIntensity;
        public double TotalMouseIntensity;
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace Herring
{
    /// Information about an application installed in the system
    public class AppInfo
    {
        public string       Name;
        public string       Path;
        public Icon         Icon;
        public ColorBin[]   ColorBins;
    }

    /// The basic piece of information about user activity
    public class ActivitySample
    {
        public AppInfo  App;
        public string   WindowTitle;
        public string   ApplicationTitle;
        public double   KeyboardIntensity;
        public double   MouseIntensity;
    }

    // Information gathered at a single time moment
    public class ActivitySnapshot : ActivitySample
    {
        public DateTime Time;   // This is the END of the time-span        

        public static double GetIntensity(double rawValue, int unit, TimeSpan span)
        {
            double minutes = span.TotalMilliseconds / 60000.0;
            double value = rawValue / unit;
            return value / minutes;
        }

        public bool IsKeyboardWarm
        {
            get { return KeyboardIntensity >= 0.5; }    // [words per minute]
        }

        public bool IsMouseWarm
        {
            get { return MouseIntensity >= 0.5; }       // [1000 pixels per minute]
        }

        public bool IsWarm
        {
            get { return IsKeyboardWarm || IsMouseWarm; }
        }
    }

    public class ActivityEntry : ActivitySample
    {
        public double Share;
    }
        
    // Information got from many time quantuums
    public class ActivitySummary
    {
        public DateTime TimePoint;  // This is the START of the time span
        public TimeSpan Span;
        public List<ActivityEntry> Entries;
        
        public double TotalShare;
        public double TotalKeyboardIntensity;
        public double TotalMouseIntensity;
    }

    class ActivityDaySummary : ActivitySample
    {
        public double TotalTime;
    }

    public enum UserStatus
    {
        Away,
        Passive,
        Active
    }

}

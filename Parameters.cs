using System;

namespace Herring
{
    enum LogTimeMode
    {
        FiveMinutes,
        FifteenMinutes,
        OneHour,
        WholeDay
    }

    static class Parameters
    {
        public static LogTimeMode LogTimeMode = LogTimeMode.FiveMinutes;
        public static int LogTimeUnit = 300;           // [s]
        public static int LogSamplingRate = 100;       // how many samples are taken for one time unit (should be at least 3)

        public static int InactivityThreshold_Idle = 30;    // [s]
        public static int InactivityThreshold_Away = 120;   // [s] after this time the user is assumed AWAY of the computer

        public static bool AutoScroll = false;

        public static double MinimumShare = 3.0;
        public static int    MaxTitleDifferenceLength = 3;
        public static double MaxTitleDifferenceRatio = 0.3;
    }
}

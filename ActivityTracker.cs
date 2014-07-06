using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Herring
{
    static class ActivityTracker
    {
        private static List<ActivitySummary> currentLog;   // being tracked right now (today)
        private static List<ActivitySummary> selectedLog;  // being displayed

        private static int logTimeUnit = 60;            // [s]
        private static int logSamplingRate = 100;       // how many samples are taken for one time unit (should be at least 3)
        private static int inactivityThreshold = 120;   // [s] after this time the user is assumed away of the computer

        private static List<ActivitySnapshot> currentSnapshots = new List<ActivitySnapshot>();
        private static DateTime lastActivityTime = DateTime.Now;
        private static bool isUserActive = true;

        public static int LogTimeUnit
        {
            get { return logTimeUnit;  }
        }

        public static int LogSamplingRate
        {
            get { return logSamplingRate; }
        }

        public static List<ActivitySummary> SelectedLog
        {
            get { return selectedLog;  }
        }

        public static void SetCurrentActivityLog()
        {
            selectedLog = currentLog;
        }

        public static void SetCurrentActivityLog(List<ActivitySummary> log)
        {
            currentLog = log;
            selectedLog = currentLog;
        }

        public static void SetSelectedActivityLog(List<ActivitySummary> log)
        {
            selectedLog = log;
        }

        private static void SetUserStatus(bool value)
        {
            if (isUserActive != value)
            {
                isUserActive = value;
                OnUserStatusChanged(isUserActive);
            }
        }

        // Returns current time rounded down to appropriate time point
        private static DateTime GetCurrentTimePoint()
        {
            return GetTimePoint(DateTime.Now, ActivityTracker.LogTimeUnit);
        }

        private static DateTime GetTimePoint(DateTime time, int timeUnit)
        {
            // We assume time units up to 15 minutes
            System.Diagnostics.Debug.Assert(timeUnit >= 1 && timeUnit <= 15 * 60);

            // There must be an integer number of time points in an hour
            System.Diagnostics.Debug.Assert(3600 % timeUnit == 0);

            int actualTotalSeconds = time.Second + time.Minute * 60;
            int neededTotalSeconds = (actualTotalSeconds / timeUnit) * timeUnit;

            return new DateTime(time.Year, time.Month, time.Day, time.Hour, neededTotalSeconds / 60, neededTotalSeconds % 60);
        }

        private static ActivitySummary GetActivitySummary(List<ActivitySnapshot> snapshots)
        {
            ActivitySummary summary =
                new ActivitySummary
                {
                    TimePoint = GetTimePoint(snapshots.First().Time, logTimeUnit),
                    Span = new TimeSpan(0, 0, logTimeUnit),
                    TotalKeyboardIntensity = (from x in snapshots select x.KeyboardIntensity).Average(),
                    TotalMouseIntensity = (from x in snapshots select x.MouseIntensity).Average(),
                    Entries = new List<ActivityEntry>()
                };

            Dictionary<string, double> processShare = new Dictionary<string, double>();

            bool[] done = new bool[snapshots.Count];
            for (int i = 0; i < snapshots.Count; ++i)
            {
                if (done[i] == false)
                {
                    int count = 1;
                    double sumKeyboard = snapshots[i].KeyboardIntensity;
                    double sumMouse = snapshots[i].MouseIntensity;
                    for (int j = i + 1; j < snapshots.Count; ++j)
                    {
                        if (snapshots[j].App.Name == snapshots[i].App.Name &&
                            snapshots[j].Title == snapshots[i].Title)
                        {
                            count++;
                            sumKeyboard += snapshots[j].KeyboardIntensity;
                            sumMouse += snapshots[j].MouseIntensity;
                            done[j] = true;
                        }
                    }
                    ActivityEntry newEntry =
                        new ActivityEntry
                        {
                            Share = 100 * count / snapshots.Count,
                            App = snapshots[i].App,
                            Title = snapshots[i].Title,
                            KeyboardIntensity = sumKeyboard / count,
                            MouseIntensity = sumMouse / count
                        };

                    if (newEntry.Share >= 3.0)  // 3%
                    {
                        summary.Entries.Add(newEntry);
                    }

                    // Counting the total share per process path
                    if (processShare.Keys.Contains(newEntry.App.Path))
                    {
                        processShare[newEntry.App.Path] += newEntry.Share;
                    }
                    else
                    {
                        processShare.Add(newEntry.App.Path, newEntry.Share);
                    }
                }
            }

            summary.Entries.Sort
            (
                (a, b) =>
                    processShare[a.App.Path] != processShare[b.App.Path]
                        ? (int)(1000 * (processShare[b.App.Path] - processShare[a.App.Path]))
                        : (int)(1000 * (b.Share - a.Share))
            );

            return summary;
        }

        public delegate void CurrentLogExtendedEventHandler(ActivitySummary summary);
        public static event CurrentLogExtendedEventHandler CurrentLogExtended;
        
        private static void OnCurrentLogExtended(ActivitySummary summary)
        {
            if (CurrentLogExtended != null)
            {
                CurrentLogExtended(summary);
            }
        }

        public delegate void CurrentLogChangedEventHandler(DateTime date);
        public static event CurrentLogChangedEventHandler CurrentLogChanged;

        private static void OnCurrentLogChanged(DateTime date)
        {
            if (CurrentLogChanged != null)
            {
                CurrentLogChanged(date);
            }
        }

        public delegate void UserStatusEventHandler(bool isActive);
        public static event UserStatusEventHandler UserStatusChanged;

        private static void OnUserStatusChanged(bool isActive)
        {
            if (UserStatusChanged != null)
            {
                UserStatusChanged(isActive);
            }
        }

        public static void RegisterSnapshot(ActivitySnapshot snapshot)
        {
            bool timePointChanged;
            bool dateChanged;
            DateTime currDate;
            if (currentSnapshots.Count == 0)
            {
                timePointChanged = false;
                dateChanged = false;
                currDate = DateTime.MinValue;   // not used
            }
            else
            {
                DateTime currTimePoint = GetTimePoint(snapshot.Time, logTimeUnit);
                DateTime prevTimePoint = GetTimePoint(currentSnapshots.Last().Time, logTimeUnit);
                timePointChanged = (currTimePoint != prevTimePoint);
                dateChanged = (currTimePoint.Date != prevTimePoint.Date);
                currDate = currTimePoint.Date;
            }

            if (snapshot.IsActive)
            {
                lastActivityTime = snapshot.Time;
                SetUserStatus(true);
            }
            else
            {
                TimeSpan inactivityTime = (snapshot.Time - lastActivityTime);
                if (inactivityTime.TotalSeconds >= inactivityThreshold)
                {
                    SetUserStatus(false);
                }
            }

            if (timePointChanged)
            {
                // summarize the previous interval
                ActivitySummary summary = GetActivitySummary(currentSnapshots);

                Persistence.Store(summary);

                currentLog.Add(summary);
                if (currentLog == selectedLog)
                {
                    OnCurrentLogExtended(summary);
                }

                currentSnapshots.Clear();
            }

            currentSnapshots.Add(snapshot);

            if (dateChanged)
            {
                System.Diagnostics.Debug.Assert(timePointChanged);

                Persistence.Close();

                currentLog.Clear();
                if (currentLog == selectedLog)
                {
                    OnCurrentLogChanged(currDate);
                }
            }

        }



    }
}

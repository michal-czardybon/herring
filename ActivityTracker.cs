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

        private static List<ActivitySnapshot> currentSnapshots = new List<ActivitySnapshot>();
        private static DateTime? prevTimePoint = null;
        private static DateTime lastActivityTime = DateTime.Now;
        private static UserStatus userStatus = UserStatus.Active;

        public static int LogTimeUnit
        {
            get { return Parameters.LogTimeUnit;  }
        }

        public static int LogSamplingRate
        {
            get { return Parameters.LogSamplingRate; }
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

        private static void SetUserStatus(UserStatus value)
        {
            if (userStatus != value)
            {
                userStatus = value;
                OnUserStatusChanged(userStatus);
            }
        }

        // Returns current time rounded down to appropriate time point
        public static DateTime GetCurrentTimePoint()
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

        private static bool AreTitlesNearlyEqual(string title1, string title2, int maxDiff, out string title)
        {
            // Count the common characters from the beginning
            int i = 0;
            while (i < title1.Length && i < title2.Length && title1[i] == title2[i])
            {
                i++;
            }

            // Count the common characters from the end
            int j = 0;
            int p = title1.Length;
            int q = title2.Length;
            while (p > i && q > i && title1[p-1] == title2[q-1])
            {
                j++;
                p--;
                q--;
            }

            // Count the different characters inside
            int diffLen1 = p - i;
            int diffLen2 = q - i;
            int diffLen = Math.Max(diffLen1, diffLen2);

            if (diffLen <= maxDiff)
            {
                title = title1.Substring(0, i) + new String('*', diffLen) + title1.Substring(p);
                return true;
            }
            else
            {
                title = "";
                return false;
            }
        }

        /*private static void Test_AreTitlesNearlyEqual()
        {
            string common;
            System.Diagnostics.Debug.Assert(AreTitlesNearlyEqual("Inbox (58) out", "Inbox (58) out", 3, out common) == true);
            System.Diagnostics.Debug.Assert(common == "Inbox (58) out");
            System.Diagnostics.Debug.Assert(AreTitlesNearlyEqual("aaaaa", "aaa", 3, out common) == true);
            System.Diagnostics.Debug.Assert(common == "aaa**");
            System.Diagnostics.Debug.Assert(AreTitlesNearlyEqual("Inbox (58) out", "Inbox (89) out", 3, out common) == true);
            System.Diagnostics.Debug.Assert(common == "Inbox (**) out");
            System.Diagnostics.Debug.Assert(AreTitlesNearlyEqual("Inbox (58) out", "Inbox (893) out", 3, out common) == true);
            System.Diagnostics.Debug.Assert(common == "Inbox (***) out");
            System.Diagnostics.Debug.Assert(AreTitlesNearlyEqual("Inbox (583) out", "Inbox (89) out", 3, out common) == true);
            System.Diagnostics.Debug.Assert(common == "Inbox (***) out");
            System.Diagnostics.Debug.Assert(AreTitlesNearlyEqual("Inbox (5832) out", "Inbox (89) out", 3, out common) == false);
            System.Diagnostics.Debug.Assert(common == "");
            System.Diagnostics.Debug.Assert(AreTitlesNearlyEqual("Inbox (*) out", "Inbox (89) out", 3, out common) == true);
            System.Diagnostics.Debug.Assert(common == "Inbox (**) out");
        }*/

        private static ActivitySummary GetActivitySummary(List<ActivitySnapshot> snapshots, DateTime timePoint)
        {
            ActivitySummary summary;
            
            if (snapshots.Count >= 1)
            {
                summary =
                    new ActivitySummary
                    {
                        TimePoint = timePoint,
                        Span = new TimeSpan(0, 0, Parameters.LogTimeUnit),
                        TotalKeyboardIntensity = (from x in snapshots select x.KeyboardIntensity).Average(),
                        TotalMouseIntensity = (from x in snapshots select x.MouseIntensity).Average(),
                        Entries = new List<ActivityEntry>()
                    };
            }
            else
            {
                summary =
                    new ActivitySummary
                    {
                        TimePoint = timePoint,
                        Span = new TimeSpan(0, 0, Parameters.LogTimeUnit),
                        TotalShare = 100.0 * snapshots.Count / Parameters.LogSamplingRate,
                        TotalKeyboardIntensity = 0,
                        TotalMouseIntensity = 0,
                        Entries = new List<ActivityEntry>()
                    };
            }
                
            Dictionary<string, double> processShare = new Dictionary<string, double>();

            bool[] done = new bool[snapshots.Count];
            for (int i = 0; i < snapshots.Count; ++i)
            {
                string thisTitle = snapshots[i].Title;
                if (done[i] == false)
                {
                    int count = 1;
                    double sumKeyboard = snapshots[i].KeyboardIntensity;
                    double sumMouse = snapshots[i].MouseIntensity;
                    for (int j = i + 1; j < snapshots.Count; ++j)
                    {
                        string commonTitle;
                        if (snapshots[j].App.Name == snapshots[i].App.Name &&
                            AreTitlesNearlyEqual(snapshots[j].Title, thisTitle, Parameters.MaxTitleDifference, out commonTitle))
                        {
                            count++;
                            sumKeyboard += snapshots[j].KeyboardIntensity;
                            sumMouse += snapshots[j].MouseIntensity;
                            done[j] = true;
                            thisTitle = commonTitle;
                        }
                    }
                    ActivityEntry newEntry =
                        new ActivityEntry
                        {
                            Share = 100 * count / Parameters.LogSamplingRate,
                            App = snapshots[i].App,
                            Title = thisTitle,
                            KeyboardIntensity = sumKeyboard / count,
                            MouseIntensity = sumMouse / count
                        };

                    if (newEntry.Share >= Parameters.MinimumShare)
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

        public delegate void UserStatusEventHandler(UserStatus status);
        public static event UserStatusEventHandler UserStatusChanged;

        private static void OnUserStatusChanged(UserStatus status)
        {
            if (UserStatusChanged != null)
            {
                UserStatusChanged(status);
            }
        }

        public static void RegisterSnapshot(ActivitySnapshot snapshot)
        {
            bool timePointChanged;
            bool dateChanged;
            DateTime currDate;
            DateTime currTimePoint = GetTimePoint(snapshot.Time, Parameters.LogTimeUnit);
            if (prevTimePoint == null)
            {
                timePointChanged = false;
                dateChanged = false;
                currDate = DateTime.MinValue;   // not used
            }
            else
            {
                timePointChanged = (currTimePoint != prevTimePoint.Value);
                dateChanged = (currTimePoint.Date != prevTimePoint.Value.Date);
                currDate = currTimePoint.Date;
            }
            prevTimePoint = currTimePoint;

            if (snapshot.IsWarm)
            {
                lastActivityTime = snapshot.Time;
                SetUserStatus(UserStatus.Active);
            }
            else
            {
                TimeSpan inactivityTime = (snapshot.Time - lastActivityTime);
                if (inactivityTime.TotalSeconds >= Parameters.InactivityThreshold_Away)
                {
                    SetUserStatus(UserStatus.Away);

                    // ...and remove previous snapshots which also had no keyboard nor mouse actions
                    // (unfortunatelly only withing the current time point)
                    while (currentSnapshots.Count >= 1 && currentSnapshots.Last().IsWarm == false)
                    {
                        currentSnapshots.RemoveAt(currentSnapshots.Count - 1);
                    }
                }
                else if (inactivityTime.TotalSeconds >= Parameters.InactivityThreshold_Idle)
                {
                    SetUserStatus(UserStatus.Passive);
                }
            }

            if (timePointChanged)
            {
                // summarize the previous interval
                ActivitySummary summary = GetActivitySummary(currentSnapshots, prevTimePoint.Value);
                Persistence.Store(summary);

                currentLog.Add(summary);
                if (currentLog == selectedLog)
                {
                    OnCurrentLogExtended(summary);
                }

                currentSnapshots.Clear();
            }

            if (userStatus == UserStatus.Active || userStatus == UserStatus.Passive)
            {
                currentSnapshots.Add(snapshot);
            }

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

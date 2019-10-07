using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Herring
{
    public static class ActivityTracker
    {
        private static Log currentLog;   // being tracked right now (today)
        private static Log selectedLog;  // being displayed

        private static List<ActivitySnapshot> activeSnapshots = new List<ActivitySnapshot>();
        private static DateTime activeTimePoint = GetTimePoint(DateTime.Now, Parameters.LogTimeUnit);

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

        public static Log SelectedLog
        {
            get { return selectedLog;  }
        }

        public static void SetCurrentActivityLog()
        {
            selectedLog = currentLog;
        }

        public static void SetCurrentActivityLog(Log log)
        {
            currentLog = log;
            selectedLog = currentLog;
        }

        public static void SetSelectedActivityLog(Log log)
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

        public static DateTime GetTimePoint(DateTime time, int timeUnit)
        {
            // We assume time units up to 15 minutes
            Debug.Assert(timeUnit >= 1 && timeUnit <= 15 * 60);

            // There must be an integer number of time points in an hour
            Debug.Assert(3600 % timeUnit == 0);

            var actualTotalSeconds = time.Second + time.Minute * 60;
            var neededTotalSeconds = (actualTotalSeconds / timeUnit) * timeUnit;

            return new DateTime(time.Year, time.Month, time.Day, time.Hour, neededTotalSeconds / 60, neededTotalSeconds % 60);
        }

        public static bool AreTitlesNearlyEqual(string title1, string title2, out string title)
        {
            return AreTitlesNearlyEqual(title1, title2, Parameters.MaxTitleDifferenceLength, out title);
        }

        public static string CommonUrlPart(string url1, string url2)
        {
            int i = 0;
            while (i < url1.Length && i < url2.Length && url1[i] == url2[i])
            {
                ++i;
            }
            return url1.Substring(0, i);

            /*int i1 = 0;
            int i2 = 0;

            while (true)
            {
                System.Diagnostics.Debug.Assert(i1 == i2);

                if (i1 == url1.Length)
                {
                    return url1.Substring(0, i1);
                }

                // Get next parts of both urls
                int j1 = url1.IndexOf("/", i1 + 1);
                int j2 = url2.IndexOf("/", i2 + 1);
                if (j1 == -1) j1 = url1.Length;
                if (j2 == -1) j2 = url2.Length;

                // If both parts are equal...
                if (url1.Substring(i1, j1 - i1) == url2.Substring(i2, j2 - i2))
                {
                    // ...go ahead...
                    i1 = j1;
                    i2 = j2;
                }
                else
                {
                    // ...or stop here otherwise.
                    return url1.Substring(0, i1);
                }
            }*/
        }

        public static bool AreTitlesNearlyEqual(string title1, string title2, int maxDiff, out string title)
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

            int equalLength = i + j;
            if (diffLen <= equalLength &&
                diffLen <= maxDiff)
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

        private static ActivitySummary GetActivitySummary(List<ActivitySnapshot> snapshots, DateTime timePoint)
        {
            ActivitySummary summary;
            
            if (snapshots.Count >= 1)
            {
                double keyboardIntensity =
                    snapshots.Count >= 1
                    ? (from x in snapshots select x.KeyboardIntensity).Average()
                    : 0.0;
                double mouseIntensity =
                    snapshots.Count >= 1
                    ? (from x in snapshots select x.MouseIntensity).Average()
                    : 0.0;
                summary =
                    new ActivitySummary
                    {
                        TimePoint = timePoint,
                        Span = new TimeSpan(0, 0, Parameters.LogTimeUnit),
                        TotalShare = 100.0 * snapshots.Count / Parameters.LogSamplingRate,
                        TotalKeyboardIntensity = keyboardIntensity,
                        TotalMouseIntensity = mouseIntensity,
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
                        TotalShare = 0,
                        TotalKeyboardIntensity = 0,
                        TotalMouseIntensity = 0,
                        Entries = new List<ActivityEntry>()
                    };
            }
                
            Dictionary<string, double> processShare = new Dictionary<string, double>();

            bool[] done = new bool[snapshots.Count];
            for (int i = 0; i < snapshots.Count; ++i)
            {
                string thisProcess = snapshots[i].App.Name;
                string thisTitle = snapshots[i].ApplicationTitle;
                string thisDocumentSite = snapshots[i].ValidDocumentSite;
                string thisCommonUrl = snapshots[i].DocumentUrl;
                if (done[i] == false)
                {
                    int count = 1;
                    double sumKeyboard = snapshots[i].KeyboardIntensity;
                    double sumMouse = snapshots[i].MouseIntensity;
                    for (int j = i + 1; j < snapshots.Count; ++j)
                    {
                        string commonTitle;
                        if (snapshots[j].App.Name == thisProcess &&
                            snapshots[j].ValidDocumentSite == thisDocumentSite &&
                            AreTitlesNearlyEqual(snapshots[j].ApplicationTitle, thisTitle, out commonTitle))
                        {
                            count++;
                            sumKeyboard += snapshots[j].KeyboardIntensity;
                            sumMouse += snapshots[j].MouseIntensity;
                            done[j] = true;
                            thisTitle = commonTitle;
                            thisCommonUrl = CommonUrlPart(thisCommonUrl, snapshots[j].DocumentUrl);
                        }
                    }
                    Debug.Assert(count >= 1);
                    ActivityEntry newEntry =
                        new ActivityEntry
                        {
                            Share = 100 * count / Parameters.LogSamplingRate,
                            App = snapshots[i].App,
                            ApplicationTitle = thisTitle,
                            WindowTitle = "",
                            DocumentUrl = thisCommonUrl,
                            KeyboardIntensity = sumKeyboard / count,
                            MouseIntensity = sumMouse / count
                        };
                    newEntry.SetCategory();

                    summary.Entries.Add(newEntry);
                    
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

        private static void SplitSnapshots(List<ActivitySnapshot> activeSnapshots, DateTime activeTimePoint, List<ActivitySnapshot> currSnapshots, List<ActivitySnapshot> nextSnapshots)
        {
            DateTime nextTimePoint = activeTimePoint.AddSeconds(Parameters.LogTimeUnit);
            foreach (ActivitySnapshot s in activeSnapshots)
            {
                if (s.Time < nextTimePoint)
                {
                    currSnapshots.Add(s);
                }
                else
                {
                    nextSnapshots.Add(s);
                }
            }
        }

        private static void MaybeCommitSummary(DateTime currTimePoint)
        {
            if (currTimePoint > activeTimePoint)
            {
                // Separate snapshots belonging to the current and the next interval
                List<ActivitySnapshot> currSnapshots = new List<ActivitySnapshot>();
                List<ActivitySnapshot> nextSnapshots = new List<ActivitySnapshot>();
                SplitSnapshots(activeSnapshots, activeTimePoint, currSnapshots, nextSnapshots);

                // Summarize the previous interval
                ActivitySummary summary = GetActivitySummary(currSnapshots, activeTimePoint);

                if (summary.Entries.Count >= 1)
                {
                    currentLog.AddSummary(summary);
                    
                    if (currentLog == selectedLog)
                    {
                        OnCurrentLogExtended(summary);
                    }
                }

                // Consider the end of the day
                bool dateChanged = (currTimePoint.Date > activeTimePoint.Date);
                if (dateChanged)
                {
                    currentLog.Clear();
                    if (currentLog == selectedLog)
                    {
                        OnCurrentLogChanged(currTimePoint.Date);
                    }
                }

                // Start a new interval
                activeSnapshots = nextSnapshots;
                activeTimePoint = currTimePoint;
            }
        }

        public static void RegisterSnapshot(ActivitySnapshot snapshot)
        {
            DateTime currTimePoint = GetTimePoint(snapshot.Time, Parameters.LogTimeUnit);

            if (snapshot.IsWarm)
            {
                lastActivityTime = snapshot.Time;
                SetUserStatus(UserStatus.Active);               
                MaybeCommitSummary(currTimePoint);  // because a warm snapshot guarantees that none of the existing snapshots will be removed
            }
            else
            {
                TimeSpan inactivityTime = (snapshot.Time - lastActivityTime);
                if (inactivityTime.TotalSeconds >= Parameters.InactivityThreshold_Away)
                {
                    SetUserStatus(UserStatus.Away);

                    // ...and remove previous snapshots which also had no keyboard nor mouse actions
                    while (activeSnapshots.Count >= 1 && activeSnapshots.Last().IsWarm == false)
                    {
                        activeSnapshots.RemoveAt(activeSnapshots.Count - 1);
                    }

                    MaybeCommitSummary(currTimePoint);  // because no further snapshots will be removed (this is a delayed commit)
                }
                else if (inactivityTime.TotalSeconds >= Parameters.InactivityThreshold_Idle)
                {
                    SetUserStatus(UserStatus.Passive);
                }
            }

            if (userStatus == UserStatus.Active || userStatus == UserStatus.Passive)
            {
                activeSnapshots.Add(snapshot);
            }
        }



    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace Herring
{
    static class Persistence
    {
        private static TextWriter writer;

        public static string GetLocalDataDir()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dir = System.IO.Path.Combine(dir, "Herring Activity Tracker");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public static string GetApplicationDir()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, "Herring Activity Tracker");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        private static void Load(GetAppDelegate getApp, string path, List<ActivitySummary> data)
        {
            TextReader reader = new StreamReader(path);
            string header = reader.ReadLine();  // csv header
            bool hasSubtitle;
            bool hasDocument;
            if (header == "time;span;process;title;subtitle;document;share;keyboard-intensity;mouse-intensity;")
            {
                hasSubtitle = true;
                hasDocument = true;
            }
            else
            if (header == "time;span;process;title;subtitle;share;keyboard-intensity;mouse-intensity;")
            {
                hasSubtitle = true;
                hasDocument = false;
            }
            else
            if (header == "time;span;process;title;share;keyboard-intensity;mouse-intensity;")
            {
                hasSubtitle = false;
                hasDocument = false;
            }
            else
            {
                throw new ApplicationException("Incorrect log file format.");
            }

            ActivitySummary lastSummary = null;
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                List<string> parts = new List<string>(line.Split(new char[] { ';' }));
                while (parts.Count < 9) parts.Add("");

                if (hasSubtitle == false)
                {
                    parts[7] = parts[6];
                    parts[6] = parts[5];
                    parts[5] = parts[4];
                    parts[4] = "";          // subtitle
                }
                if (hasDocument == false)
                {
                    parts[8] = parts[7];
                    parts[7] = parts[6];
                    parts[6] = parts[5];
                    parts[5] = parts[4];
                    parts[4] = "";          // document
                }

                if (parts[0] != "")
                {
                    double keyboardIntensity;
                    double mouseIntensity;
                    double.TryParse(parts[7], out keyboardIntensity);
                    double.TryParse(parts[8], out mouseIntensity);

                    ActivitySummary summary = new ActivitySummary()
                    {
                        TimePoint = DateTime.Parse(parts[0]),
                        Span = TimeSpan.Parse(parts[1]),
                        TotalShare = double.Parse(parts[6]),
                        TotalKeyboardIntensity = keyboardIntensity,
                        TotalMouseIntensity = mouseIntensity,
                        Entries = new List<ActivityEntry>()
                    };
                    data.Add(summary);
                    lastSummary = summary;
                }
                else
                {
                    if (lastSummary == null)
                    {
                        throw new ApplicationException("First entry in the log file is not a summary.");
                    }
                    ActivityEntry entry =
                        new ActivityEntry()
                        {
                            App = getApp(parts[2]),
                            ApplicationTitle = parts[3],
                            WindowTitle = parts[4],
                            DocumentName = parts[5]
                        };
                    System.Diagnostics.Debug.Assert(entry.DocumentName != null);
                    try
                    {
                        entry.Share = double.Parse(parts[6]);
                        entry.KeyboardIntensity = double.Parse(parts[7]);
                        entry.MouseIntensity = double.Parse(parts[8]);
                    }
                    catch (FormatException)
                    {
                        entry.Share = 0;
                        entry.KeyboardIntensity = 0;
                        entry.MouseIntensity = 0;
                    }
                    entry.SetCategory();
                    lastSummary.Entries.Add(entry);
                }
            }

            reader.Close();
        }

        public static List<ActivitySummary> Load(GetAppDelegate getApp, DateTime date)
        {
            List<ActivitySummary> data = new List<ActivitySummary>();

            string path = GetLocalDataDir();
            string filePattern = String.Format("herring{0:D4}{1:D2}{2:D2}_*.*", date.Year, date.Month, date.Day);

            string[] files = Directory.GetFiles(path, filePattern);
            foreach (string f in files)
            {
                Load(getApp, f, data);
            }

            return data;
        }

        public static List<ActivitySummary> Load(GetAppDelegate getApp)
        {
            return Load(getApp, DateTime.Now);
        }

        private static string ConstructFileName(DateTime date, bool full)
        {
            string name = String.Format("herring{0:D4}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.csv", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            string path = Path.Combine(GetLocalDataDir(), name);
            return path;
        }

        public static void Store(ActivitySummary data)
        {
            if (writer == null)
            {
                string path = ConstructFileName(DateTime.Now, true);
                writer = new StreamWriter(path);
                writer.WriteLine("time;span;process;title;subtitle;document;share;keyboard-intensity;mouse-intensity;");
            }

            writer.Write(data.TimePoint.ToString() + ";");
            writer.Write(data.Span.ToString() + ";");
            writer.Write(";");
            writer.Write(";");
            writer.Write(";");
            writer.Write(";");
            writer.Write(data.TotalShare.ToString("F2") + ";");
            writer.Write(data.TotalKeyboardIntensity.ToString("F2") + ";");
            writer.Write(data.TotalMouseIntensity.ToString("F2") + ";");
            writer.WriteLine();

            foreach (ActivityEntry entry in data.Entries)
            {
                writer.Write(";");
                writer.Write(";");
                writer.Write(entry.App.Path + ";");
                writer.Write(entry.ApplicationTitle.Replace(';', ',') + ";");
                writer.Write(entry.WindowTitle.Replace(';', ',') + ";");
                writer.Write(entry.DocumentName.Replace(';', ',') + ";");
                writer.Write(entry.Share.ToString("F2") + ";");
                writer.Write(entry.KeyboardIntensity.ToString("F2") + ";");
                writer.Write(entry.MouseIntensity.ToString("F2") + ";");
                writer.WriteLine();
            }

            writer.Flush();
        }

        public static void Close()
        {
            if (writer != null)
            {
                writer.Close();
                writer = null;
            }
        }
    }
}

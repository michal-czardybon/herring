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

        private static void Load(GetAppDelegate getApp, string path, List<ActivitySummary> data, ref List<string> errorLines)
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

            int lineNumber = 0;

            ActivitySummary lastSummary = null;
            while (true)
            {
                string line = reader.ReadLine();
                lineNumber++;
                if (line == null)
                    break;

                try
                {
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

                    CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                    DateTimeStyles styles = DateTimeStyles.None;

                    double keyboardIntensity;
                    double mouseIntensity;
                    if (double.TryParse(parts[7], out keyboardIntensity) == false)
                    {
                        // Try English
                        if (double.TryParse(parts[7], NumberStyles.Float, culture, out keyboardIntensity) == false)
                        {
                            throw new ApplicationException("Cannot parse a real number.");
                        }
                    }
                    if (double.TryParse(parts[8], out mouseIntensity) == false)
                    {
                        // Try English
                        if (double.TryParse(parts[8], NumberStyles.Float, culture, out mouseIntensity) == false)
                        {
                            throw new ApplicationException("Cannot parse a real number.");
                        }
                    }

                    double share;
                    if (double.TryParse(parts[6], out share) == false)
                    {
                        // Try English
                        if (double.TryParse(parts[6], NumberStyles.Float, culture, out share) == false)
                        {
                            throw new ApplicationException("Cannot parse a real number.");
                        }
                    }

                    if (parts[0] != "")
                    {
                        DateTime date;
                        if (DateTime.TryParse(parts[0], out date) == false)
                        {
                            // Try English
                            if (DateTime.TryParse(parts[0], culture, styles, out date) == false)
                            {
                                throw new ApplicationException("Cannot parse a date string.");
                            }
                        }

                        ActivitySummary summary = new ActivitySummary();
                        summary.TimePoint = date;
                        summary.Span = TimeSpan.Parse(parts[1]);
                        summary.TotalShare = share;
                        summary.TotalKeyboardIntensity = keyboardIntensity;
                        summary.TotalMouseIntensity = mouseIntensity;
                        summary.Entries = new List<ActivityEntry>();
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
                        entry.Share = share;
                        entry.KeyboardIntensity = keyboardIntensity;
                        entry.MouseIntensity = mouseIntensity;
                        entry.SetCategory();
                        lastSummary.Entries.Add(entry);
                    }
                }
                catch(Exception)
                {
                    errorLines.Add(lineNumber.ToString("D3") + ":" + (line ?? "UNKNOWN") );
                }
            }

            reader.Close();
        }

        public static List<ActivitySummary> Load(GetAppDelegate getApp, DateTime date, out List<string> errors)
        {
            List<ActivitySummary> data = new List<ActivitySummary>();

            string path = GetLocalDataDir();
            string filePattern = String.Format("herring{0:D4}{1:D2}{2:D2}_*.*", date.Year, date.Month, date.Day);

            string[] files = Directory.GetFiles(path, filePattern);
            List<string> errorLines = new List<string>();
            foreach (string f in files)
            {
                Load(getApp, f, data, ref errorLines);
            }

            errors = errorLines;

            return data;
        }

        public static List<ActivitySummary> Load(GetAppDelegate getApp, out List<string> errors)
        {
            return Load(getApp, DateTime.Now, out errors);
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
                writer.Write(entry.ApplicationTitle.Replace(';', ',').Replace('\r',' ').Replace("\n", " ") + ";");
                writer.Write(entry.WindowTitle.Replace(';', ',').Replace('\r', ' ').Replace("\n", " ") + ";");
                writer.Write(entry.DocumentName.Replace(';', ',').Replace('\r', ' ').Replace("\n", " ") + ";");
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

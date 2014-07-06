using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace Herring
{
    class Persistence
    {
        private static TextWriter writer;

        private static string GetApplicationDir()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, "Herring");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        private static void Load(GetAppDelegate getApp, string path, List<ActivitySummary> data)
        {
            TextReader reader = new StreamReader(path);
            string header = reader.ReadLine();  // csv header
            if (header != "time;span;process;title;share;keyboard-intensity;mouse-intensity;")
            {
                throw new ApplicationException("Incorrect log file format.");
            }

            ActivitySummary lastSummary = null;
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                string[] parts = line.Split(new char[] { ';' });

                if (parts[0] != "")
                {
                    ActivitySummary summary = new ActivitySummary()
                    {
                        TimePoint = DateTime.Parse(parts[0]),
                        Span = TimeSpan.Parse(parts[1]),
                        TotalShare = int.Parse(parts[4]),
                        TotalMouseIntensity = int.Parse(parts[5]),
                        TotalKeyboardIntensity = int.Parse(parts[6]),
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
                    ActivityEntry entry = new ActivityEntry()
                    {
                        App = getApp(parts[2]),
                        Title = parts[3],
                        Share = int.Parse(parts[4]),
                        MouseIntensity = int.Parse(parts[5]),
                        KeyboardIntensity = int.Parse(parts[6])
                    };
                    lastSummary.Entries.Add(entry);
                }
            }

            reader.Close();
        }

        public static List<ActivitySummary> Load(GetAppDelegate getApp, DateTime date)
        {
            List<ActivitySummary> data = new List<ActivitySummary>();

            string path = GetApplicationDir();
            string filePattern = String.Format("herring{0:D4}{1:D2}{2:D2}_*.txt", date.Year, date.Month, date.Day);

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
            string name = String.Format("herring{0:D4}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.txt", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            string path = Path.Combine(GetApplicationDir(), name);
            return path;
        }

        public static void Store(ActivitySummary data)
        {
            if (writer == null)
            {
                string path = ConstructFileName(DateTime.Now, true);
                writer = new StreamWriter(path);
                writer.WriteLine("time;span;process;title;share;keyboard-intensity;mouse-intensity;");
            }

            writer.Write(data.TimePoint.ToString() + ";");
            writer.Write(data.Span.ToString() + ";");
            writer.Write(";");
            writer.Write(";");
            writer.Write(data.TotalShare + ";");
            writer.Write(data.TotalKeyboardIntensity + ";");
            writer.Write(data.TotalMouseIntensity + ";");
            writer.WriteLine();

            foreach (ActivityEntry entry in data.Entries)
            {
                writer.Write(";");
                writer.Write(";");
                writer.Write(entry.App.Path + ";");
                writer.Write(entry.Title + ";");
                writer.Write(entry.Share + ";");
                writer.Write(entry.KeyboardIntensity + ";");
                writer.Write(entry.MouseIntensity + ";");
                writer.WriteLine();
            }

            writer.Flush();
        }

        public static void Close()
        {
            writer.Close();
        }
    }
}

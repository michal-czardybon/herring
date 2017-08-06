using System;
using System.IO;

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

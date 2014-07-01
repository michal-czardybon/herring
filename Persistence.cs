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

        private static void Load(GetAppDelegate getApp, string path, List<ActivitySnapshot> data)
        {
            TextReader reader = new StreamReader(path);
            string header = reader.ReadLine();  // csv header
            if (header != "time;span;process;title;mouse-clicks;mouse-travel;keys-pressed;")
            {
                throw new ApplicationException("Incorrect log file format.");
            }

            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                string[] parts = line.Split(new char[] { ';' });
                ActivitySnapshot snapshot = new ActivitySnapshot();
                snapshot.Begin = DateTime.Parse(parts[0]);
                snapshot.Length = TimeSpan.Parse(parts[1]);
                snapshot.App = getApp(parts[2]);
                snapshot.Title = parts[3];
                snapshot.MouseClickCount = int.Parse(parts[4]);
                snapshot.MouseMoveDistance = int.Parse(parts[5]);
                snapshot.KeyPressCount = int.Parse(parts[6]);

                data.Add(snapshot);
            }

            reader.Close();
        }

        public static List<ActivitySnapshot> Load(GetAppDelegate getApp, DateTime date)
        {
            List<ActivitySnapshot> data = new List<ActivitySnapshot>();

            string path = GetApplicationDir();
            string filePattern = String.Format("herring{0:D4}{1:D2}{2:D2}_*.txt", date.Year, date.Month, date.Day);

            string[] files = Directory.GetFiles(path, filePattern);
            foreach (string f in files)
            {
                Load(getApp, f, data);
            }

            return data;
        }

        public static List<ActivitySnapshot> Load(GetAppDelegate getApp)
        {
            return Load(getApp, DateTime.Now);
        }

        private static string ConstructFileName(DateTime date, bool full)
        {
            string name = String.Format("herring{0:D4}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.txt", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            string path = Path.Combine(GetApplicationDir(), name);
            return path;
        }

        public static void Store(ActivitySnapshot data)
        {
            if (writer == null)
            {
                string path = ConstructFileName(DateTime.Now, true);
                writer = new StreamWriter(path);
                writer.WriteLine("time;span;process;title;mouse-clicks;mouse-travel;keys-pressed;");
            }
            writer.Write(data.Begin.ToString() + ";");
            writer.Write(data.Length.ToString() + ";");
            writer.Write(data.App.Name + ";");
            writer.Write(data.Title + ";");
            writer.Write(data.MouseClickCount + ";");
            writer.Write((int)data.MouseMoveDistance + ";");
            writer.Write(data.KeyPressCount + ";");
            writer.WriteLine();
            writer.Flush();
        }

        public static void Close()
        {
            writer.Close();
        }
    }
}

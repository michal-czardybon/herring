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
            if (header != "time;process;title;mouse-intensity;clicking-intensity;typing-intensity")
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
                snapshot.App = getApp(parts[1]);
                snapshot.Title = parts[2];
                //snapshot.MouseSpeed = int.Parse(parts[3]);
                //snapshot.ClickingSpeed = int.Parse(parts[4]);
                //snapshot.TypingSpeed = int.Parse(parts[5]);

                data.Add(snapshot);
            }

            reader.Close();
        }

        public static List<ActivitySnapshot> Load(GetAppDelegate getApp, DateTime date)
        {
            List<ActivitySnapshot> data = new List<ActivitySnapshot>();

            string path = GetApplicationDir();
            string filePattern = "herring" + date.Year + date.Month + date.Day + "*.txt";

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
            string name = "herring" + date.Year + date.Month + date.Day + "_" + date.Hour + date.Minute + date.Second + ".txt";
            string path = Path.Combine(GetApplicationDir(), name);
            return path;
        }

        public static void Store(ActivitySnapshot data)
        {
            if (writer == null)
            {
                string path = ConstructFileName(DateTime.Now, true);
                writer = new StreamWriter(path);
                writer.WriteLine("time;process;title;mouse-intensity;clicking-intensity;typing-intensity");
            }
            writer.Write(data.Begin.ToString() + ";");
            writer.Write(data.App.Name + ";");
            writer.Write(data.Title + ";");
            writer.Write(data.MouseSpeed + ";");
            writer.Write(data.ClickingSpeed + ";");
            writer.Write(data.TypingSpeed + ";");
            writer.WriteLine();
            writer.Flush();
        }

        public static void Close()
        {
            writer.Close();
        }
    }
}

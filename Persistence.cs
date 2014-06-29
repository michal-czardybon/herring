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

            // TODO

            reader.Close();
        }

        public static List<ActivitySnapshot> Load(GetAppDelegate getApp)
        {
            List<ActivitySnapshot> data = new List<ActivitySnapshot>();

            string path = GetApplicationDir();
            string[] files = Directory.GetFiles(path, "herring*.txt");
            foreach (string f in files)
            {
                Load(getApp, f, data);
            }
            return data;
        }

        public static void Store(ActivitySnapshot data)
        {
            if (writer == null)
            {
                DateTime now = DateTime.Now;
                string name = "herring" + now.Year + now.Month + now.Day + "_" + now.Hour + now.Minute + now.Second + ".txt";
                string path = Path.Combine(GetApplicationDir(), name);
                writer = new StreamWriter(path);
                writer.WriteLine("time;process;title;mouse-intensity;clicking-intensity;typing-intensity");
            }
            writer.Write(data.App.Name + ";");
            writer.Write(data.Title + ";");
            writer.Write(data.MouseSpeed + ";");
            writer.Write(data.ClickingSpeed + ";");
            writer.Write(data.TypingSpeed + ";");
            writer.Flush();
        }

        public static void Close()
        {
            writer.Close();
        }
    }
}

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

        private static string ReadXmlValue(TextReader reader)
        {
            string line = reader.ReadLine();
            int pos1 = line.IndexOf('"');
            int pos2 = line.IndexOf('"', pos1 + 1);
            return line.Substring(pos1 + 1, pos2 - pos1 - 1);
        }

        private static void Load(GetAppDelegate getApp, string path, List<ActivitySnapshot> data)
        {
            TextReader reader = new StreamReader(path);
            reader.ReadLine();  // xml header
            reader.ReadLine();  // <Herring>
            while (true)
            {
                string begin = reader.ReadLine();   // <ActivitySnapshot
                if (begin != "\t<ActivitySnapshot")
                {
                    break;
                }
                else
                {
                    ActivitySnapshot snapshot = new ActivitySnapshot();
                    string appPath = ReadXmlValue(reader);
                    snapshot.App = getApp(appPath);
                    snapshot.Title = ReadXmlValue(reader);
                    snapshot.Begin = DateTime.Parse( ReadXmlValue(reader), CultureInfo.InvariantCulture );
                    snapshot.Length = TimeSpan.Parse(ReadXmlValue(reader));
                    snapshot.CharsTyped = ReadXmlValue(reader);
                    snapshot.MouseDistance = int.Parse( ReadXmlValue(reader) );
                    snapshot.MouseClicks = int.Parse(ReadXmlValue(reader));
                    data.Add(snapshot);
                }
            }
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
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<HerringData>");
            }
            writer.WriteLine("\t<ActivitySnapshot");
            writer.WriteLine("\t\tAppPath=\"" + data.App.Path + "\"");
            writer.WriteLine("\t\tTitle=\"" + data.Title + "\"");
            writer.WriteLine("\t\tBegin=\"" + data.Begin.ToString(CultureInfo.InvariantCulture) + "\"");
            writer.WriteLine("\t\tLength=\"" + data.Length + "\"");
            writer.WriteLine("\t\tCharsTyped=\"" + data.CharsTyped + "\"");
            writer.WriteLine("\t\tMouseDistance=\"" + (int)data.MouseDistance + "\"");
            writer.WriteLine("\t\tMouseClicks=\"" + data.MouseClicks + "\">");
            writer.Flush();
        }

        public static void Close()
        {
            writer.WriteLine("</HerringData>");
            writer.Close();
        }
    }
}

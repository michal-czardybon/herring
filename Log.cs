using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Herring
{

    /// <summary>
    /// Class contains all data asociated with daily activity.
    /// </summary>
    public class Log
    {

        public enum EventType
        {
            None,
            AwayFromComputer,
            WorkignHours
        }

        public class Event
        {
            public DateTime Start;

            [XmlIgnore]
            public TimeSpan Span;

            public string SpanTime
            {
                get
                {
                    return XmlConvert.ToString(Span);
                }

                set
                {
                    Span = string.IsNullOrEmpty(value) ?
                        TimeSpan.Zero : XmlConvert.ToTimeSpan(value);
                }
            }

            public string Description;
            public EventType Type;

            public Event(DateTime Start, TimeSpan Span, string Description, EventType Type)
            {
                this.Start = Start;
                this.Span = Span;
                this.Description = Description;
                this.Type = Type;
            }

            public Event()
            {
                this.Start = DateTime.Now;
                this.Span = new TimeSpan();
                this.Description = null;
                this.Type = EventType.None;
            }

            public bool ContainsTimePoint(DateTime time)
            {
                var period = time - Start;

                return period > TimeSpan.Zero && period <= Span;
            }
        }

        [XmlIgnore]
        public List<ActivitySummary> Activities = new List<ActivitySummary>();

        public List<Event> Events = new List<Event>();

        public Log()
        {
        }

        public static Log Load(GetAppDelegate getApp, DateTime date, out List<string> errors)
        {
            Log data = new Log();

            string path = Persistence.GetLocalDataDir();
            string filePattern = String.Format("herring{0:D4}{1:D2}{2:D2}_*.*", date.Year, date.Month, date.Day);

            string[] files = Directory.GetFiles(path, filePattern);
            List<string> errorLines = new List<string>();
            foreach (string f in files)
            {
                LoadPart(getApp, f, data.Activities, errorLines);
            }

            errors = errorLines;

            data.LoadEvents(date);

            return data;
        }

        private static void LoadPart(GetAppDelegate getApp, string path, List<ActivitySummary> data, List<string> errorLines)
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
                catch (Exception)
                {
                    errorLines.Add(lineNumber.ToString("D3") + ":" + (line ?? "UNKNOWN"));
                }
            }

            reader.Close();
        }

        public void AddSummary(ActivitySummary summary)
        {
            Persistence.Store(summary);
            Activities.Add(summary);
        }

        public void Clear()
        {
            Persistence.Close();
            Activities.Clear();
        }

        private void LoadEvents(DateTime date)
        {
            string path = Persistence.GetLocalDataDir();
            var name = String.Format("herring_events{0:D4}{1:D2}{2:D2}.xml", date.Year, date.Month, date.Day);

            var serializer = new XmlSerializer(typeof(Log));
            var eventsFile = Path.Combine(path, name);


            if (!File.Exists(eventsFile))
            {
                return;
            }

            using (var reader = new StreamReader(eventsFile))
            {
                using (var xml = XmlReader.Create(reader))
                {
                    var data = (Log)serializer.Deserialize(xml);

                    Events.Clear();
                    Events.AddRange(data.Events);

                }
            }

        }

        private void StoreEvents(DateTime date)
        {
            string path = Persistence.GetLocalDataDir();

            var name = String.Format("herring_events{0:D4}{1:D2}{2:D2}.xml", date.Year, date.Month, date.Day);

            var serializer = new XmlSerializer(typeof(Log));
            using (var writer = new StreamWriter(Path.Combine(path, name)))
            {
                using (var xml = XmlWriter.Create(writer))
                {
                    serializer.Serialize(writer, this);
                }
            }

        }

        public void MarkWorkingHours(DateTime time, TimeSpan span)
        {
            var e = new Event(time, span, "Working hours", EventType.WorkignHours);
            Events.Add(e);
            StoreEvents(time);
        }

        public void AddAwayFromComputer(DateTime time, TimeSpan span, string description)
        {
            var e = new Event(time, span, description, EventType.AwayFromComputer);
            Events.Add(e);
            StoreEvents(time);
        }

        public void RemoveEvent(Event e)
        {
            Events.Remove(e);
            StoreEvents(e.Start);
        }
    }
}

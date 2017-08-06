using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Herring
{
    class Rule
    {
        public string Process = "";
        public string Title = "";
        public string Document = "";
        public double KeyboardMin = 0.0;
        public double KeyboardMax = double.PositiveInfinity;
        public double MouseMin = 0.0;
        public double MouseMax = double.PositiveInfinity;
        public UserStatus StatusMin = UserStatus.Passive;
        public UserStatus StatusMax = UserStatus.Active;

        public string Category;
    }

    static class RuleManager
    {
        public static List<Rule> Rules = new List<Rule>();
        public static List<string> Categories = new List<string>();

        static private UserStatus ParseUserStatus(string value)
        {
            switch (value.ToUpper())
            {
                case "AWAY": return UserStatus.Away;
                case "PASSIVE": return UserStatus.Passive;
                case "IDLE": return UserStatus.Passive;     // backward compatibility (to be removed)
                case "ACTIVE": return UserStatus.Active;
                default: throw new ApplicationException("Unknown user status.");
            }
        }

        static public void Load()
        {
            string path = Path.Combine(Persistence.GetApplicationDir(), "Rules.txt");

            if (File.Exists(path))
            {
                TextReader reader = new StreamReader(path);
                //try
                {
                    while (true)
                    {

                        string line = reader.ReadLine();
                        if (line == null) break;
                        if (line == "") continue;
                        if (line.StartsWith("//")) continue;

                        string[] parts = line.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 2)
                        {
                            throw new ApplicationException("Incorrect format of rules.");
                        }
                        string conditions = parts[0].Trim();
                        string category = parts[1].Trim();

                        Rule rule = new Rule();
                        rule.Category = category;

                        if (Categories.Contains(category) == false)
                        {
                            Categories.Add(category);
                        }

                        MatchCollection matches = Regex.Matches(conditions, @"(?<field>[a-zA-Z_\-]+)\:((""(?<value>([^""]*))"")|(?<value>\S*))");
                        foreach (Match m in matches)
                        {
                            string field = m.Groups["field"].Value;
                            string value = m.Groups["value"].Value;
                            switch (field)
                            {
                                case "process":
                                    rule.Process = value;
                                    break;
                                case "title":
                                    rule.Title = value;
                                    break;
                                case "document":
                                    rule.Document = value;
                                    break;
                                case "keyboard-min":
                                    rule.KeyboardMin = double.Parse(value);
                                    break;
                                case "keyboard-max":
                                    rule.KeyboardMax = double.Parse(value);
                                    break;
                                case "mouse-min":
                                    rule.MouseMin = double.Parse(value);
                                    break;
                                case "mouse-max":
                                    rule.MouseMax = double.Parse(value);
                                    break;
                                case "status-min":
                                    rule.StatusMin = ParseUserStatus(value);
                                    break;
                                case "status-max":
                                    rule.StatusMax = ParseUserStatus(value);
                                    break;
                            }
                        }
                        Rules.Add(rule);
                    }
                }
                /*catch
                {
                    // TODO
                }*/
                reader.Close();
            }
        }

        public static string MatchCategory(ActivitySample sample)
        {
            foreach (var rule in Rules)
            {
                if (sample.App.Name.ToLower().Contains(rule.Process.ToLower()) &&
                    sample.ApplicationTitle.ToLower().Contains(rule.Title.ToLower()) &&
                    sample.ValidDocumentName.ToLower().Contains(rule.Document.ToLower()) &&
                    //sample.WindowTitle.Contains(rule.Title) &&
                    sample.KeyboardIntensity >= rule.KeyboardMin &&
                    sample.KeyboardIntensity <= rule.KeyboardMax &&
                    sample.MouseIntensity >= rule.MouseMin &&
                    sample.MouseIntensity <= rule.MouseMax)
                {
                    return rule.Category;
                }
            }
            return "";
        }

    }
}

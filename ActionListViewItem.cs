using System.Drawing;
using System.Windows.Forms;

namespace Herring
{
   public partial class MainForm
   {
      public class ActionListViewItem: ListViewItem
        {
            private ActivitySummary summary;
            private ListViewItem header;
            private ActivityEntry entry;

            public ActivitySummary Summary
            {
                get
                {
                    return summary;
                }
            }

            public ListViewItem Header
            {
                get
                {
                    return header;
                }
            }

            private void AddColumn(string s)
            {
                SubItems.Add(s);
            }

            private void AddColumn(float f)
            {
                SubItems.Add(f.ToString("F1"));
            }

            private void AddColumn(double d)
            {
                SubItems.Add(d.ToString("F1"));
            }

            private void AddColumn(int i)
            {
                SubItems.Add(i.ToString());
            }

            public ActionListViewItem(ActivityEntry entry, int imgIndex, ActivitySummary summary, ListViewItem header)
            {
                this.summary = summary;
                this.header = header;
                this.entry = entry;

                ImageIndex = imgIndex;

                ForeColor =
                    entry.Share >= 20.0 ? Color.Black :
                    entry.Share >= 10.0 ? Color.FromArgb(64, 64, 64) :
                                          Color.FromArgb(128, 128, 128);
                
                // Remove uneccesary extension
                var processedName = entry.App.Name.ToLower().Replace(".exe", "");

                Text = processedName;                /* process: */
                AddColumn(entry.ApplicationTitle);    /* title: */
                AddColumn(entry.Subtitle);            /* subtitle: */
                AddColumn(entry.Share);               /* share: */
                AddColumn(entry.KeyboardIntensity);   /* keyboard: */
                AddColumn(entry.MouseIntensity);      /* mouse: */
                AddColumn(entry.Category);            /* category: */

                UseItemStyleForSubItems = false;
                
                for (int i = 0; i < SubItems.Count - 1; ++i)
                {
                    SubItems[i].ForeColor = ForeColor;
                }

                SubItems[SubItems.Count - 1].ForeColor = Chart.GetColor(entry.CategoryIndex + 1);
            }

            public void SelectHeader()
            {
                Header.Selected = true;
                Header.EnsureVisible();
            }
        }
    }
}

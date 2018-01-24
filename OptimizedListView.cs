using System;
using System.Linq;
using System.Windows.Forms;

namespace Herring
{
    class OptimizedListView: ListView
    {
        public OptimizedListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

        }

        protected override void WndProc(ref Message m)
        {
            const int WM_ERASEBKGND = 0x0014;

            if (m.Msg == WM_ERASEBKGND)
            {
                m.Msg = (int)IntPtr.Zero;
            }
            base.WndProc(ref m);
        }

        public void ClearSelection()
        {
            foreach (var i in Items.OfType<ListViewItem>())
            {
                i.Selected = false;
            }
        }
    }
}

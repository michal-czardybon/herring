using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Herring
{
   internal static class Helpers
   {
      private const int WM_SETREDRAW = 11;

      [DllImport("user32.dll")]
      public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

      public static void Suspend(Control parent)
      {
         SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
      }

      public static void Resume(Control parent)
      {
         SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
         parent.Refresh();
      }
      
   }
}

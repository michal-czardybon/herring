using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Automation;
using System.Text.RegularExpressions;

namespace Herring
{
    class SystemInfo
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowTextLength(IntPtr hWnd);
        
        //  int GetWindowText(
        //      __in   HWND hWnd,
        ////      __out  LPTSTR lpString,
        //      __in   int nMaxCount
        //  );
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        //  DWORD GetWindowThreadProcessId(
        //      __in   HWND hWnd,
        //      __out  LPDWORD lpdwProcessId
        //  );
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //  UINT WINAPI GetWindowModuleFileName(
        //    _In_   HWND hwnd,
        //    _Out_  LPTSTR lpszFileName,
        //    _In_   UINT cchFileNameMax
        //  );
        [DllImport("user32.dll")]
        private static extern uint GetWindowModuleFileName(IntPtr hWnd, StringBuilder lpszFileName, uint cchFileNameMax);

        // HWND WINAPI GetParent(
        //   _In_  HWND hWnd
        // );
        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        //HWND WINAPI GetAncestor(
        //  _In_  HWND hwnd,
        //  _In_  UINT gaFlags
        //);
        [DllImport("user32.dll")]
        private static extern IntPtr GetAncestor(IntPtr hWnd, uint gaFlags);

        //HWND WINAPI GetWindow(
        //  _In_  HWND hWnd,
        //  _In_  UINT uCmd
        //);
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        
        //HANDLE WINAPI OpenProcess(
        //  __in  DWORD dwDesiredAccess,
        //  __in  BOOL bInheritHandle,
        //  __in  DWORD dwProcessId
        //);
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        //  DWORD WINAPI GetModuleBaseName(
        //      __in      HANDLE hProcess,
        //      __in_opt  HMODULE hModule,
        //      __out     LPTSTR lpBaseName,
        //      __in      DWORD nSize
        //  );
        [DllImport("psapi.dll")]
        private static extern uint GetModuleBaseName(IntPtr hWnd, IntPtr hModule, StringBuilder lpFileName, int nSize);

        //  DWORD WINAPI GetModuleFileNameEx(
        //      __in      HANDLE hProcess,
        //      __in_opt  HMODULE hModule,
        //      __out     LPTSTR lpFilename,
        //      __in      DWORD nSize
        //  );
        [DllImport("psapi.dll")]
        private static extern uint GetModuleFileNameEx(IntPtr hWnd, IntPtr hModule, StringBuilder lpFileName, int nSize);

        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        private static string GetWindowTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            StringBuilder text = new StringBuilder(length + 1);
            GetWindowText(hWnd, text, text.Capacity);
            string ret = text.ToString();
            if (ret.EndsWith("- Google Chrome"))
            {
                ret = ret.Substring(0, ret.Length - 15);
            }
            return ret;
        }

        public static string GetChromeUrl()
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return "(ZERO)";

            AutomationElement elm = AutomationElement.FromHandle(hWnd);

            // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
            AutomationElement elmUrlBar = null;
            try
            {
                /*var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                if (elm1 == null) { return "(NOT RIGHT)"; } // not the right chrome.exe
                var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1); // I don't know a Condition for this for finding
                var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                var elm4 = TreeWalker.RawViewWalker.GetNextSibling(elm3); // I don't know a Condition for this for finding
                var elm7 = elm4.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
                elmUrlBar = elm7.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));*/

                var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                if (elm1 == null) { return "(NOT RIGHT)"; }  // not the right chrome.exe
                var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1);
                var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.HelpTextProperty, "TopContainerView"));
                var elm4 = elm3.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
                var elm5 = elm4.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.HelpTextProperty, "LocationBarView"));
                elmUrlBar = elm5.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            }
            catch
            {
                // Chrome has probably changed something, and above walking needs to be modified. :(
                // put an assertion here or something to make sure you don't miss it
                return "(EXCEPTION)";
            }

            // make sure it's valid
            if (elmUrlBar == null)
            {
                // it's not..
                return "(INVALID 1)";
            }

            // elmUrlBar is now the URL bar element. we have to make sure that it's out of keyboard focus if we want to get a valid URL
            if ((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
            {
                return "(INVALID 2)";
            }

            // there might not be a valid pattern to use, so we have to make sure we have one
            AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
            if (patterns.Length == 1)
            {
                string ret = "";
                try
                {
                    ret = ((ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0])).Current.Value;
                }
                catch { }
                if (ret != "")
                {
                    // must match a domain name (and possibly "https://" in front)
                    if (Regex.IsMatch(ret, @"^(https:\/\/)?[a-zA-Z0-9\-\.]+(\.[a-zA-Z]{2,4}).*$"))
                    {
                        // prepend http:// to the url, because Chrome hides it if it's not SSL
                        if (!ret.StartsWith("http"))
                        {
                            ret = "http://" + ret;
                        }
                        return ret;
                    }
                }
            }
            return "(FAILED)";
        }

        public static void GetTopWindowText(out string windowText, out string applicationText)
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd != IntPtr.Zero)
            {
                windowText = GetWindowTitle(hWnd);

                IntPtr hWndOwner = hWnd;
                do
                {
                    applicationText = GetWindowTitle(hWndOwner);
                    hWndOwner = GetWindow(hWndOwner, 4); // 4 == GW_OWNER
                }
                while (hWndOwner != IntPtr.Zero);

                if (applicationText == windowText)
                {
                    windowText = "";
                }
            }
            else
            {
                windowText = "";
                applicationText = "";
            }
        }

        public static string GetTopWindowPath()
        {
            IntPtr hWnd = GetForegroundWindow();
            uint lpdwProcessId;
            GetWindowThreadProcessId(hWnd, out lpdwProcessId);

            IntPtr hProcess = OpenProcess(0x0410, false, lpdwProcessId);

            if (hProcess != IntPtr.Zero)
            {
                StringBuilder text = new StringBuilder(1000);
                GetModuleFileNameEx(hProcess, IntPtr.Zero, text, text.Capacity);

                CloseHandle(hProcess);
                return text.ToString();
            }
            else
            {
                return "UNKNOWN";
            }
        }
    }
}

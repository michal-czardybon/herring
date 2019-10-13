using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Text.RegularExpressions;
using System.Diagnostics;

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

            // Remove an annoying suffix in Google Chrome
            if (ret.EndsWith("- Google Chrome"))
            {
                ret = ret.Substring(0, ret.Length - 15);
            }
            return ret;
        }

        private static IntPtr prevHWnd = IntPtr.Zero;
        private static AutomationElement prevElm0 = null;
        private static AutomationElement prevElmFinal = null;
        private static bool prevIsIncognito = false;
        private static double urlTime = 0;

        public static string GetChromeUrl()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return "(No window)";
            AutomationElement elm0 = AutomationElement.FromHandle(hWnd);
            if (elm0 == null) return ("No automation");

            // Method 1 (dirty)
            //AutomationElement elmFinal = AutomationElement.FromPoint(new System.Windows.Point(200, 60));

            // Method 2
            AutomationElement elmFinal;
            bool isIncognito;

            if (hWnd == prevHWnd && elm0 == prevElm0)
            {
                // Optimization for speed
                elmFinal = prevElmFinal;
                isIncognito = prevIsIncognito;
            }
            else
            {
                try
                {
                    var elm1 = elm0.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                    if (elm1 == null)
                    {
                        return ("(LostChrome)");
                    }
                    var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1);
                    var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                    var elm4 = elm3.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""))[1];

                    AutomationElement elmIncognito = elm4.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Incognito"));
                    isIncognito = (elmIncognito != null);

                    if (isIncognito)
                    {
                        elmFinal = null;
                    }
                    else
                    {
                        var elm5 = elm4.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                        var elm6 = elm5.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                        elmFinal = elm6;
                    }
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // I can come here by closing the Chrome window between getting elm2, elm3, elm4
                    prevHWnd = IntPtr.Zero;
                    prevElm0 = null;
                    prevElmFinal = null;
                    return "(LostTrack)";
                }
                catch (System.IndexOutOfRangeException)
                {
                    prevHWnd = IntPtr.Zero;
                    prevElm0 = null;
                    prevElmFinal = null;
                    return "(LostIndex)";
                }
                catch (System.NullReferenceException)
                {
                    prevHWnd = IntPtr.Zero;
                    prevElm0 = null;
                    prevElmFinal = null;
                    return "(NullElement)";
                }

                prevHWnd = hWnd;
                prevElm0 = elm0;
                prevElmFinal = elmFinal;
                prevIsIncognito = isIncognito;
            }

            string result;
            if (isIncognito)
            {
                result = "(Incognito)";
            }
            else
            {
                // elmUrlBar is now the URL bar element. we have to make sure that it's out of keyboard focus if we want to get a valid URL
                try
                {
                    if ((bool)elmFinal.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
                    {
                        result = "(HasKeyboard)";
                    }
                    else
                    {
                        result =
                            elmFinal == null ? "(NULL)" : ((ValuePattern)elmFinal.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                    }
                }
                catch (System.Windows.Automation.ElementNotAvailableException)
                {
                    result = "(PatternError)";
                }
                catch (System.NullReferenceException)
                {
                    result = "(NullElement)";
                }
            }

            urlTime = sw.ElapsedMilliseconds;

            return result;


            // Method 3 (more general, but slower)
            /*Condition conditions = new AndCondition(
                new PropertyCondition(AutomationElement.IsContentElementProperty, true),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            AutomationElement elmFinal = elm0.FindFirst(TreeScope.Descendants, conditions);*/
        }

        public static double GetUrlRetrievalTime()
        {
            return urlTime;
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

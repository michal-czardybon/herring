using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

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

        private static string GetWindowTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            StringBuilder text = new StringBuilder(length + 1);
            GetWindowText(hWnd, text, text.Capacity);
            return text.ToString();
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
                while (hWndOwner != IntPtr.Zero) ;
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

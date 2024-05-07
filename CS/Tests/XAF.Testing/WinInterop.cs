using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Text;

namespace XAF.Testing{
    public static class WinInterop{
        public const uint MonitorDefaultToNearest = 2;
        public const int DwmwaExtendedFrameBounds = 9;
        public const int SwRestore = 9;
        public const uint SwpNozorder = 0x0004;
        public const uint WmPaint = 0x000F;
        public const int SwMaximize = 3;
        private static readonly IntPtr HwndTopmost = new(-1);
        private static readonly IntPtr HwndNoTopMost = new(-2);
        private const uint SwpNoMove = 0x0002;
        private const uint SwpNoSize = 0x0001;
        private const uint EVENT_OBJECT_CREATE = 0x8000;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private static WinEventDelegate winEventDelegate; // Keep a reference to delegate to avoid garbage collection
        private static IntPtr winEventHookHandle;
        private static Subject<IntPtr> newWindowSubject = new Subject<IntPtr>();
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder stringBuilder, int cch);
        public static string WindowTitle(this IntPtr hwnd){
            var stringBuilder = new StringBuilder(256);
            GetWindowText(hwnd, stringBuilder, 256);
            return stringBuilder.ToString();
        }

         
        private const int WmClose = 0x0010;

        

        public static void CloseWindow(this IntPtr hWnd) => SendMessage(hWnd, WmClose, IntPtr.Zero, IntPtr.Zero);

        public static IObservable<IntPtr> CloseWindow(this string title)
            => title.WhenNewWindowTitle().Do(ptr => ptr.CloseWindow());
        
        public static IObservable<IntPtr> WhenNewWindowTitle(this string title) 
            => Observable.Create<IntPtr>(observer => {
                    winEventDelegate = (_, eventType, hwnd, _, _, _, _) => {
                        if (eventType != EVENT_OBJECT_CREATE) return;
                        observer.OnNext(hwnd);
                    };
                    winEventHookHandle = SetWinEventHook(EVENT_OBJECT_CREATE, EVENT_OBJECT_CREATE, IntPtr.Zero, winEventDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
                    return () => {
                        UnhookWinEvent(winEventHookHandle);
                        winEventDelegate = null;
                    };
                }).Publish().RefCount().Where(ptr => ptr.WindowTitle().Contains(title));

        public static void AlwaysOnTop(this IntPtr hWnd, bool enable=true) 
            => SetWindowPos(hWnd, enable ? HwndTopmost : HwndNoTopMost, 0, 0, 0, 0, SwpNoMove | SwpNoSize);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect{
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        
        
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT{
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Monitorinfo{
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref Monitorinfo lpmi);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        public static Rectangle ToRectangle(this RECT rect) 
            => new(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
    }
}
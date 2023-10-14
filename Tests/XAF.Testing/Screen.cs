// using System.Drawing;
// using System.Runtime.InteropServices;
//
// namespace XAF.Testing;
// internal class Screen
// {
//     
//     [DllImport("user32.dll")]
//     private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);
//
//     [DllImport("user32.dll", SetLastError = true)]
//     private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
//
//     [DllImport("user32.dll", SetLastError = true)]
//     private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
//
//     private const uint MONITOR_DEFAULTTOPRIMARY = 1;
//
//     public static Screen PrimaryScreen { get; } = new Screen(MonitorFromWindow(IntPtr.Zero, MONITOR_DEFAULTTOPRIMARY));
//
//     private IntPtr monitorHandle;
//     private Screen(IntPtr monitorHandle)
//     {
//         this.monitorHandle = monitorHandle;
//     }
//
//     public Rectangle Bounds
//     {
//         get
//         {
//             MONITORINFO info = new MONITORINFO();
//             info.cbSize = (uint)Marshal.SizeOf(info);
//             GetMonitorInfo(monitorHandle, ref info);
//             return new Rectangle(info.rcMonitor.Left, info.rcMonitor.Top, info.rcMonitor.Right - info.rcMonitor.Left, info.rcMonitor.Bottom - info.rcMonitor.Top);
//         }
//     }
//
//     public static Screen FromHandle(IntPtr hWnd)
//     {
//         IntPtr monitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTOPRIMARY);
//         return new Screen(monitor);
//     }
//
//     [StructLayout(LayoutKind.Sequential)]
//     private struct POINT
//     {
//         public int X;
//         public int Y;
//     }
//
//     [StructLayout(LayoutKind.Sequential)]
//     private struct RECT
//     {
//         public int Left;
//         public int Top;
//         public int Right;
//         public int Bottom;
//     }
//
//     [StructLayout(LayoutKind.Sequential)]
//     private struct MONITORINFO
//     {
//         public uint cbSize;
//         public RECT rcMonitor;
//         public RECT rcWork;
//         public uint dwFlags;
//     }
//
//     private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);
// }
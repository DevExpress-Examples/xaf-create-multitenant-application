using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace XAF.Testing{
    public static class ScreenCapture{
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
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT{
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        

        public static RECT GetScreenBounds(this IntPtr hWnd) 
            => GetWindowRect(hWnd, out RECT rect) ? rect
                : throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());


        private const int SwRestore = 9;
        private const uint SwpNozorder = 0x0004;
        private const uint WmPaint = 0x000F;
        private const int SwMaximize = 3;

        private const int DwmwaExtendedFrameBounds = 9;
        public static Bitmap CaptureActiveWindow(){
            var hWnd = GetForegroundWindow();
            return hWnd != IntPtr.Zero ? DwmGetWindowAttribute(hWnd, DwmwaExtendedFrameBounds, out var rect, Marshal.SizeOf(typeof(RECT))) == 0
                ? rect.Capture() : null : null;
        }
        public static Bitmap Capture(this RECT rect) 
            => new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top).Capture();

        public static Bitmap Capture(this Rectangle bounds){
            var result = new Bitmap(bounds.Width, bounds.Height);
            using var g = Graphics.FromImage(result);
            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            return result;
        }
        
        public static void MoveActiveWindowToMainMonitorAndWaitForRender(){
            var hWnd = GetForegroundWindow();
            // if (hWnd == IntPtr.Zero||Screen.FromHandle(hWnd).Equals(Screen.PrimaryScreen)) return;
            // ShowWindow(hWnd, SwRestore);
            // var mainMonitorBounds = Screen.PrimaryScreen.Bounds;
            // SetWindowPos(hWnd, IntPtr.Zero, mainMonitorBounds.Left, mainMonitorBounds.Top, 0, 0, SwpNozorder);
            throw new NotImplementedException();
            SendMessage(hWnd, WmPaint, IntPtr.Zero, IntPtr.Zero);
            UpdateWindow(hWnd);
            SetForegroundWindow(hWnd);
            ShowWindow(hWnd, SwMaximize);
            SendMessage(hWnd, WmPaint, IntPtr.Zero, IntPtr.Zero);
            UpdateWindow(hWnd);
        }    
        
        public static string CaptureActiveWindowAndSave(string path=null){
            MoveActiveWindowToMainMonitorAndWaitForRender();
            var filename = path??Path.GetTempFileName().Replace(".tmp", ".bmp");
            CaptureActiveWindow().Save(filename);
            return filename;
        }
    }
}
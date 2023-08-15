using System.Runtime.InteropServices;

namespace XAF.Testing;
public static class ScreenCapture
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static Bitmap CaptureActiveWindow()
    {
        IntPtr hWnd = GetForegroundWindow();
        if (hWnd == IntPtr.Zero)
            return null;

        if (!GetWindowRect(hWnd, out RECT rect))
            return null;

        Rectangle bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        Bitmap result = new Bitmap(bounds.Width, bounds.Height);

        using (Graphics g = Graphics.FromImage(result))
        {
            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        }

        return result;
    }

    public static void CaptureActiveWindowAndSave(string path)
    {
        Bitmap bmp = CaptureActiveWindow();
        bmp?.Save(path);
    }
}
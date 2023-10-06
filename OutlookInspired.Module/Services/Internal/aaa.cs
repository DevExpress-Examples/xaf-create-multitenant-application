using System.Drawing;

// You may need to reference System.Drawing.Common assembly for this

namespace OutlookInspired.Module.Services.Internal;
public static class ColorGenerator
{
    public static IEnumerable<string> DistinctColors(this int i)
    {
        return Enumerable.Range(0,i).Select(i1 => Color.FromArgb(GetRGB(i1)).ToHex());
    }

    public static string ToHex(this Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
    public static int GetRGB(int index)
    {
        int[] p = GetPattern(index);
        return (GetElement(p[0]) << 16) | (GetElement(p[1]) << 8) | GetElement(p[2]);
    }

    public static int GetElement(int index)
    {
        int value = index - 1;
        int v = 0;
        for (int i = 0; i < 8; i++)
        {
            v = v | (value & 1);
            v <<= 1;
            value >>= 1;
        }
        v >>= 1;
        return v & 0xFF;
    }

    public static int[] GetPattern(int index)
    {
        int n = (int)Math.Cbrt(index);
        index -= (n * n * n);
        int[] p = new int[3];
        Array.Fill(p, n);
        if (index == 0)
        {
            return p;
        }
        index--;
        int v = index % 3;
        index = index / 3;
        if (index < n)
        {
            p[v] = index % n;
            return p;
        }
        index -= n;
        p[v] = index / n;
        p[++v % 3] = index % n;
        return p;
    }
}
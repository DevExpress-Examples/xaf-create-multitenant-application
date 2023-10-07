using System.Drawing;

namespace OutlookInspired.Module.Services.Internal{
    public static class ColorGenerator{
        public static IEnumerable<string> DistinctColors(this int i) 
            => Enumerable.Range(0, i).Select(i1 => Color.FromArgb(GetRGB(i1)).ToHex());

        public static string ToHex(this Color color) 
            => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        static int GetRGB(int index){
            var p = GetPattern(index);
            return (GetElement(p[0]) << 16) | (GetElement(p[1]) << 8) | GetElement(p[2]);
        }

        static int GetElement(int index){
            var value = index - 1;
            var v = 0;
            for (var i = 0; i < 8; i++){
                v |= (value & 1);
                v <<= 1;
                value >>= 1;
            }
            v >>= 1;
            return v & 0xFF;
        }

        static int[] GetPattern(int index){
            var n = (int)Math.Cbrt(index);
            index -= n * n * n;
            var p = new int[3];
            Array.Fill(p, n);
            if (index == 0) return p;
            index--;
            var v = index % 3;
            index /= 3;
            if (index < n){
                p[v] = index % n;
                return p;
            }

            index -= n;
            p[v] = index / n;
            p[++v % 3] = index % n;
            return p;
        }
    }
}
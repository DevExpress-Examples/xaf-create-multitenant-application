using System.Text;

namespace XAF.Testing{
    public static class PrimitiveExtensions{
        public static byte[] Bytes(this string s, Encoding encoding = null) 
            => s == null ? Array.Empty<byte>() : (encoding ?? Encoding.UTF8).GetBytes(s);
    }
}
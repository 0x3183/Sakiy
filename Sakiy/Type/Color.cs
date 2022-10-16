using System.Runtime.InteropServices;

namespace Sakiy.Type
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Color
    {
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;
        [FieldOffset(3)]
        public byte A;
        [FieldOffset(0)]
        public uint Value;
        public Color()
        {
            Value = 0xFFFFFFFF;
            R = 0xFF;
            G = 0xFF;
            B = 0xFF;
            A = 0xFF;
        }
        public Color(byte gray)
        {
            Value = 0xFFFFFFFF;
            R = gray;
            G = gray;
            B = gray;
            A = 0xFF;
        }
        public Color(byte gray, byte alpha)
        {
            Value = 0xFFFFFFFF;
            R = gray;
            G = gray;
            B = gray;
            A = alpha;
        }
        public Color(byte red, byte green, byte blue)
        {
            Value = 0xFFFFFFFF;
            R = red;
            G = green;
            B = blue;
            A = 0xFF;
        }
        public Color(byte red, byte green, byte blue, byte alpha)
        {
            Value = 0xFFFFFFFF;
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }
    }
}

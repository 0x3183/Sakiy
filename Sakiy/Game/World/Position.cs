namespace Sakiy.Game.World
{
    public struct Position
    {
        public ulong Data { get; set; }
        public Position()
        {
            Data = 0;
        }
        public Position(int x, int y, int z)
        {
            Data = 0;
            X = x;
            Y = y;
            Z = z;
        }
        public int X
        {
            get
            {
                int Value = (int)(Data >> 38);
                if((Value & 0x02000000) != 0) Value -= 0x04000000;
                return Value;
            }
            set
            {
                ulong Value = ((ulong)value) & 0x8000000001FFFFFFUL;
                Data &= 0x0000003FFFFFFFFFUL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000002000000UL;
                Data |= (Value & 0x0000000003FFFFFFUL) << 38;
            }
        }
        public int Z
        {
            get
            {
                int Value = (int)(Data & 0x3FFFFFF);
                if ((Value & 0x02000000) != 0) Value -= 0x4000000;
                return Value;
            }
            set
            {
                ulong Value = ((ulong)value) & 0x8000000001FFFFFFUL;
                Data &= 0xFFFFFFFFFC000000UL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000002000000UL;
                Data |= Value & 0x0000000003FFFFFFUL;
            }
        }
        public int Y
        {
            get
            {
                int Value = (int)((Data >> 26) & 0xFFF);
                if ((Value & 0x00000800) != 0) Value -= 0x1000;
                return Value;
            }
            set
            {
                ulong Value = ((ulong)value) & 0x80000000000007FFUL;
                Data &= 0xFFFFFFC003FFFFFFUL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000000000800UL;
                Data |= (Value & 0x0000000000000FFFUL) << 26;
            }
        }
    }
}

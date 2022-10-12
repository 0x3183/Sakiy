using System.Text;

namespace Sakiy.Util
{
    public sealed class Encoder
    {
        internal Stream BaseStream;
        internal Encoder(Stream baseStream)
        {
            BaseStream = baseStream;
        }
        public void WriteBuffer(byte[] data, bool reverse)
        {
            byte[] buffer = data;
            if (reverse)
            {
                buffer = new byte[buffer.Length];
                Array.Copy(data, buffer, buffer.Length);
            }
            BaseStream.Write(buffer);
        }
        public void WriteBool(bool data)
        {
            WriteBuffer(BitConverter.GetBytes(data), false);
        }
        public void WriteByte(byte data)
        {
            WriteBuffer(new byte[1] { data }, false);
        }
        public void WriteSByte(sbyte data)
        {
            WriteBuffer(new byte[1] { (byte)data }, false);
        }
        public void WriteUShort(ushort data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteShort(short data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteUInt(uint data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteInt(int data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteUShort(ulong data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteLong(long data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteVarInt(int data)
        {
            while (true)
            {
                if ((data & ~0x7F) == 0)
                {
                    WriteByte((byte)data);
                    return;
                }
                WriteByte((byte)(data & 0x7F | 0x80));
                data >>= 7;
            }
        }
        public void WriteString(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            WriteVarInt(buffer.Length);
            WriteBuffer(buffer, false);
        }
    }
}

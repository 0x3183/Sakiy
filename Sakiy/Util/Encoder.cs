using System.Security.Cryptography;
using System.Text;

namespace Sakiy.Util
{
    public sealed class Encoder : IDisposable
    {
        private Stream BaseStream;
        internal Encoder(Stream baseStream)
        {
            BaseStream = baseStream;
        }
        public void Encrypt(byte[] sharedSecret)
        {
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CFB;
            aes.Padding = PaddingMode.None;
            aes.KeySize = 128;
            aes.FeedbackSize = 8;
            aes.Key = sharedSecret;
            aes.IV = sharedSecret;
            BaseStream = new CryptoStream(BaseStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        }
        public void Dispose()
        {
            BaseStream.Dispose();
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
        public void WriteULong(ulong data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteLong(long data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteFloat(float data)
        {
            WriteBuffer(BitConverter.GetBytes(data), BitConverter.IsLittleEndian);
        }
        public void WriteDouble(double data)
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
        public void WriteGuid(Guid data)
        {
            Decoder decoder = new(new MemoryStream(data.ToByteArray()));
            if (BitConverter.IsLittleEndian)
            {
                WriteBuffer(decoder.ReadBuffer(4, false), true);
                WriteBuffer(decoder.ReadBuffer(2, false), true);
                WriteBuffer(decoder.ReadBuffer(2, false), true);
                WriteBuffer(decoder.ReadBuffer(8, false), false);
            }
            else
            {
                WriteBuffer(decoder.ReadBuffer(16, false), false);
            }
            decoder.Dispose();
        }
    }
}

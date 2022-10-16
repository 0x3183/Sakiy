using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtString : NbtTag
    {
        public string Value;
        public NbtString() : base() => Value = string.Empty;
        public NbtString(string init) : base() => Value = init;
        public NbtString(Decoder decoder) : base(decoder) => Value = System.Text.Encoding.UTF8.GetString(decoder.ReadBuffer(decoder.ReadUShort(), false));
        public override void Serialize(Encoder encoder)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Value);
            if(buffer.Length > 32768) Array.Resize(ref buffer, short.MaxValue);
            encoder.WriteShort((short)buffer.Length);
            encoder.WriteBuffer(buffer, false);
        }
        public override NbtByte GetByte() => new();
        public override NbtShort GetShort() => new();
        public override NbtInt GetInt() => new();
        public override NbtLong GetLong() => new();
        public override NbtFloat GetFloat() => new();
        public override NbtDouble GetDouble() => new();
        public override NbtByteArray GetByteArray() => new();
        public override NbtString GetString() => this;
        public override NbtList<T> GetList<T>() => new();
        public override NbtCompound GetCompound() => new();
        public override NbtIntArray GetIntArray() => new();
        public override NbtLongArray GetLongArray() => new();
    }
}

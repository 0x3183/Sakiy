using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtInt : NbtTag
    {
        public int Value;
        public NbtInt() : base() => Value = 0;
        public NbtInt(int init) : base() => Value = init;
        public NbtInt(Decoder decoder) : base(decoder) => Value = decoder.ReadInt();
        public override void Serialize(Encoder encoder) => encoder.WriteInt(Value);
        public override NbtByte GetByte()
        {
            if (Value <= sbyte.MinValue) return new(sbyte.MinValue);
            if (Value >= sbyte.MaxValue) return new(sbyte.MaxValue);
            return new((sbyte)Value);
        }
        public override NbtShort GetShort()
        {
            if (Value <= short.MinValue) return new(short.MinValue);
            if (Value >= short.MaxValue) return new(short.MaxValue);
            return new((short)Value);
        }
        public override NbtInt GetInt() => this;
        public override NbtLong GetLong() => new(Value);
        public override NbtFloat GetFloat() => new(Value);
        public override NbtDouble GetDouble() => new(Value);
        public override NbtByteArray GetByteArray() => new();
        public override NbtString GetString() => new();
        public override NbtList<T> GetList<T>() => new();
        public override NbtCompound GetCompound() => new();
        public override NbtIntArray GetIntArray() => new();
        public override NbtLongArray GetLongArray() => new();
    }
}

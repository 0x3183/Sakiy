using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtShort : NbtTag
    {
        public short Value;
        public NbtShort() : base() => Value = 0;
        public NbtShort(short init) : base() => Value = init;
        public NbtShort(Decoder decoder) : base(decoder) => Value = decoder.ReadShort();
        public override void Serialize(Encoder encoder) => encoder.WriteShort(Value);
        public override NbtByte GetByte()
        {
            if (Value <= sbyte.MinValue) return new(sbyte.MinValue);
            if (Value >= sbyte.MaxValue) return new(sbyte.MaxValue);
            return new((sbyte)Value);
        }
        public override NbtShort GetShort() => this;
        public override NbtInt GetInt() => new(Value);
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

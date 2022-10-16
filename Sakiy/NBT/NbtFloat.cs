using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtFloat : NbtTag
    {
        public float Value;
        public NbtFloat() : base() => Value = 0.0F;
        public NbtFloat(float init) : base() => Value = init;
        public NbtFloat(Decoder decoder) : base(decoder) => Value = decoder.ReadFloat();
        public override void Serialize(Encoder encoder) => encoder.WriteFloat(Value);
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
        public override NbtInt GetInt()
        {
            if (Value <= int.MinValue) return new(int.MinValue);
            if (Value >= int.MaxValue) return new(int.MaxValue);
            return new((int)Value);
        }
        public override NbtLong GetLong()
        {
            if (Value <= long.MinValue) return new(long.MinValue);
            if (Value >= long.MaxValue) return new(long.MaxValue);
            return new((long)Value);
        }
        public override NbtFloat GetFloat() => this;
        public override NbtDouble GetDouble() => new(Value);
        public override NbtByteArray GetByteArray() => new();
        public override NbtString GetString() => new();
        public override NbtList<T> GetList<T>() => new();
        public override NbtCompound GetCompound() => new();
        public override NbtIntArray GetIntArray() => new();
        public override NbtLongArray GetLongArray() => new();
    }
}

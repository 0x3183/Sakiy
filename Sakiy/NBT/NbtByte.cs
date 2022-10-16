using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtByte : NbtTag
    {
        public sbyte Value;
        public NbtByte() : base() => Value = 0;
        public NbtByte(sbyte init) : base() => Value = init;
        public NbtByte(bool init) : base() => Value = (sbyte)(init ? 1 : 0);
        public NbtByte(Decoder decoder) : base(decoder) => Value = decoder.ReadSByte();
        public override void Serialize(Encoder encoder) => encoder.WriteSByte(Value);
        public override NbtByte GetByte() => this;
        public override NbtShort GetShort() => new(Value);
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

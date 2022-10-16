using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtByteArray : NbtTag
    {
        public sbyte[] Value;
        public NbtByteArray() : base() => Value = Array.Empty<sbyte>();
        public NbtByteArray(sbyte[] init) : base() => Value = init;
        public NbtByteArray(Decoder decoder) : base(decoder)
        {
            Value = new sbyte[decoder.ReadInt()];
            for (int i = 0; i < Value.Length; i++) Value[i] = decoder.ReadSByte();
        }
        public override void Serialize(Encoder encoder)
        {
            encoder.WriteInt(Value.Length);
            for (short i = 0; i < Value.Length; i++) encoder.WriteSByte(Value[i]);
        }
        public override NbtByte GetByte() => new();
        public override NbtShort GetShort() => new();
        public override NbtInt GetInt() => new();
        public override NbtLong GetLong() => new();
        public override NbtFloat GetFloat() => new();
        public override NbtDouble GetDouble() => new();
        public override NbtByteArray GetByteArray() => this;
        public override NbtString GetString() => new();
        public override NbtList<T> GetList<T>() => new();
        public override NbtCompound GetCompound() => new();
        public override NbtIntArray GetIntArray() => new();
        public override NbtLongArray GetLongArray() => new();
    }
}

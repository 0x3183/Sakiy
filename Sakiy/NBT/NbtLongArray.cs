using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtLongArray : NbtTag
    {
        public long[] Value;
        public NbtLongArray() : base() => Value = Array.Empty<long>();
        public NbtLongArray(long[] init) : base() => Value = init;
        public NbtLongArray(Decoder decoder) : base(decoder)
        {
            Value = new long[decoder.ReadInt()];
            for (int i = 0; i < Value.Length; i++) Value[i] = decoder.ReadLong();
        }
        public override void Serialize(Encoder encoder)
        {
            encoder.WriteInt(Value.Length);
            for (short i = 0; i < Value.Length; i++) encoder.WriteLong(Value[i]);
        }
        public override NbtByte GetByte() => new();
        public override NbtShort GetShort() => new();
        public override NbtInt GetInt() => new();
        public override NbtLong GetLong() => new();
        public override NbtFloat GetFloat() => new();
        public override NbtDouble GetDouble() => new();
        public override NbtByteArray GetByteArray() => new();
        public override NbtString GetString() => new();
        public override NbtList<T> GetList<T>() => new();
        public override NbtCompound GetCompound() => new();
        public override NbtIntArray GetIntArray() => new();
        public override NbtLongArray GetLongArray() => this;
    }
}

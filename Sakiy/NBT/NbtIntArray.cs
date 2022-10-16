using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtIntArray : NbtTag
    {
        public int[] Value;
        public NbtIntArray() : base() => Value = Array.Empty<int>();
        public NbtIntArray(int[] init) : base() => Value = init;
        public NbtIntArray(Decoder decoder) : base(decoder)
        {
            Value = new int[decoder.ReadInt()];
            for (int i = 0; i < Value.Length; i++) Value[i] = decoder.ReadInt();
        }
        public override void Serialize(Encoder encoder)
        {
            encoder.WriteInt(Value.Length);
            for (short i = 0; i < Value.Length; i++) encoder.WriteInt(Value[i]);
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
        public override NbtIntArray GetIntArray() => this;
        public override NbtLongArray GetLongArray() => new();
    }
}

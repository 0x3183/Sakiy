using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtList<T> : NbtTag where T : NbtTag
    {
        public readonly List<T> Value;
        public readonly byte Type;
        public NbtList() : base()
        {
            Value = new List<T>();
            Type = 0;
            if (typeof(T).IsAssignableTo(typeof(NbtByte))) Type = 1;
            if (typeof(T).IsAssignableTo(typeof(NbtShort))) Type = 2;
            if (typeof(T).IsAssignableTo(typeof(NbtInt))) Type = 3;
            if (typeof(T).IsAssignableTo(typeof(NbtLong))) Type = 4;
            if (typeof(T).IsAssignableTo(typeof(NbtFloat))) Type = 5;
            if (typeof(T).IsAssignableTo(typeof(NbtDouble))) Type = 6;
            if (typeof(T).IsAssignableTo(typeof(NbtByteArray))) Type = 7;
            if (typeof(T).IsAssignableTo(typeof(NbtString))) Type = 8;
            if (typeof(T).IsGenericType) if (typeof(T).GetGenericTypeDefinition().IsAssignableTo(typeof(NbtList<>))) Type = 9;
            if (typeof(T).IsAssignableTo(typeof(NbtCompound))) Type = 10;
            if (typeof(T).IsAssignableTo(typeof(NbtIntArray))) Type = 11;
            if (typeof(T).IsAssignableTo(typeof(NbtLongArray))) Type = 12;
            if (Type == 0) throw new ArgumentOutOfRangeException("Type (T)", Type, "Invalid NbtTag Type");
        }
        public NbtList(IEnumerable<T> init) : base()
        {
            Value = init.ToList();
            Type = 0;
            if (typeof(T).IsAssignableTo(typeof(NbtByte))) Type = 1;
            if (typeof(T).IsAssignableTo(typeof(NbtShort))) Type = 2;
            if (typeof(T).IsAssignableTo(typeof(NbtInt))) Type = 3;
            if (typeof(T).IsAssignableTo(typeof(NbtLong))) Type = 4;
            if (typeof(T).IsAssignableTo(typeof(NbtFloat))) Type = 5;
            if (typeof(T).IsAssignableTo(typeof(NbtDouble))) Type = 6;
            if (typeof(T).IsAssignableTo(typeof(NbtByteArray))) Type = 7;
            if (typeof(T).IsAssignableTo(typeof(NbtString))) Type = 8;
            if (typeof(T).IsGenericType) if (typeof(T).GetGenericTypeDefinition().IsAssignableTo(typeof(NbtList<>))) Type = 9;
            if (typeof(T).IsAssignableTo(typeof(NbtCompound))) Type = 10;
            if (typeof(T).IsAssignableTo(typeof(NbtIntArray))) Type = 11;
            if (typeof(T).IsAssignableTo(typeof(NbtLongArray))) Type = 12;
            if (Type == 0) throw new ArgumentOutOfRangeException("Type (T)", Type, "Invalid NbtTag Type");
        }
        public NbtList(Decoder decoder) : base(decoder)
        {
            Type = 0;
            if (typeof(T).IsAssignableTo(typeof(NbtByte))) Type = 1;
            if (typeof(T).IsAssignableTo(typeof(NbtShort))) Type = 2;
            if (typeof(T).IsAssignableTo(typeof(NbtInt))) Type = 3;
            if (typeof(T).IsAssignableTo(typeof(NbtLong))) Type = 4;
            if (typeof(T).IsAssignableTo(typeof(NbtFloat))) Type = 5;
            if (typeof(T).IsAssignableTo(typeof(NbtDouble))) Type = 6;
            if (typeof(T).IsAssignableTo(typeof(NbtByteArray))) Type = 7;
            if (typeof(T).IsAssignableTo(typeof(NbtString))) Type = 8;
            if (typeof(T).IsGenericType) if (typeof(T).GetGenericTypeDefinition().IsAssignableTo(typeof(NbtList<>))) Type = 9;
            if (typeof(T).IsAssignableTo(typeof(NbtCompound))) Type = 10;
            if (typeof(T).IsAssignableTo(typeof(NbtIntArray))) Type = 11;
            if (typeof(T).IsAssignableTo(typeof(NbtLongArray))) Type = 12;
            if (Type == 0) throw new ArgumentOutOfRangeException("Type (T)", Type, "Invalid NbtTag Type");
            Value = new(decoder.ReadInt());
            for (int i = 0; i < Value.Count; i++) Value.Add(Deserialize(decoder, Type) as T ?? throw new InvalidDataException());
        }
        public override void Serialize(Encoder encoder)
        {
            encoder.WriteByte(Type);
            encoder.WriteInt(Value.Count);
            foreach (T item in Value) item.Serialize(encoder);
        }
        public override NbtByte GetByte() => new();
        public override NbtShort GetShort() => new();
        public override NbtInt GetInt() => new();
        public override NbtLong GetLong() => new();
        public override NbtFloat GetFloat() => new();
        public override NbtDouble GetDouble() => new();
        public override NbtByteArray GetByteArray() => new();
        public override NbtString GetString() => new();
        public override NbtList<LT> GetList<LT>()
        {
            if (typeof(T).IsAssignableTo(typeof(LT))) return this as NbtList<LT> ?? throw new InvalidDataException();
            return new();
        }
        public override NbtCompound GetCompound() => new();
        public override NbtIntArray GetIntArray() => new();
        public override NbtLongArray GetLongArray() => new();
    }
}

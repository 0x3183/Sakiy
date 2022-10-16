using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public class NbtCompound : NbtTag
    {
        public Dictionary<string, NbtTag> Value;
        public NbtCompound(Dictionary<string, NbtTag>? init = null) : base() => Value = init ?? new();
        public NbtCompound(Decoder decoder) : base(decoder)
        {
            Value = new Dictionary<string, NbtTag>();
            while (true)
            {
                byte typeid = decoder.ReadByte();
                if (typeid == 0) break;
                string name = Deserialize(decoder, 8).GetString().Value;
                Value[name] = Deserialize(decoder, typeid);
            }
        }
        public override void Serialize(Encoder encoder)
        {
            foreach(KeyValuePair<string,NbtTag> item in Value)
            {
                byte type = 0;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtByte))) type = 1;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtShort))) type = 2;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtInt))) type = 3;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtLong))) type = 4;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtFloat))) type = 5;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtDouble))) type = 6;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtByteArray))) type = 7;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtString))) type = 8;
                if(item.Value.GetType().IsGenericType) if (item.Value.GetType().GetGenericTypeDefinition().IsAssignableTo(typeof(NbtList<>))) type = 9;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtCompound))) type = 10;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtIntArray))) type = 11;
                if (item.Value.GetType().IsAssignableTo(typeof(NbtLongArray))) type = 12;
                if (type == 0) throw new InvalidDataException();
                encoder.WriteByte(type);
                NbtString nbtname = new(item.Key);
                nbtname.Serialize(encoder);
                item.Value.Serialize(encoder);
            }
            encoder.WriteSByte(0);
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
        public override NbtCompound GetCompound() => this;
        public override NbtIntArray GetIntArray() => new();
        public override NbtLongArray GetLongArray() => new();

        public NbtByte GetByte(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtByte());
            return Value[id].GetByte();
        }
        public NbtShort GetShort(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtShort());
            return Value[id].GetShort();
        }
        public NbtInt GetInt(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtInt());
            return Value[id].GetInt();
        }
        public NbtLong GetLong(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtLong());
            return Value[id].GetLong();
        }
        public NbtFloat GetFloat(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtFloat());
            return Value[id].GetFloat();
        }
        public NbtDouble GetDouble(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtDouble());
            return Value[id].GetDouble();
        }
        public NbtByteArray GetByteArray(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtByteArray());
            return Value[id].GetByteArray();
        }
        public NbtString GetString(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtString());
            return Value[id].GetString();
        }
        public NbtList<T> GetList<T>(string id) where T : NbtTag
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtList<T>());
            return Value[id].GetList<T>();
        }
        public NbtCompound GetCompound(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtCompound());
            return Value[id].GetCompound();
        }
        public NbtIntArray GetIntArray(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtIntArray());
            return Value[id].GetIntArray();
        }
        public NbtLongArray GetLongArray(string id)
        {
            if (!Value.ContainsKey(id)) Value.Add(id, new NbtLongArray());
            return Value[id].GetLongArray();
        }
        public void Set(string id, NbtTag tag)
        {
            Value[id] = tag;
        }
    }
}

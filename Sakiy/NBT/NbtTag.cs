using Sakiy.Util;

namespace Sakiy.Game.NBT
{
    public abstract class NbtTag
    {
        internal NbtTag()
        {

        }
        internal NbtTag(Decoder decoder)
        {

        }
        public virtual void Serialize(Encoder encoder)
        {
            
        }
        public abstract NbtByte GetByte();
        public abstract NbtShort GetShort();
        public abstract NbtInt GetInt();
        public abstract NbtLong GetLong();
        public abstract NbtFloat GetFloat();
        public abstract NbtDouble GetDouble();
        public abstract NbtByteArray GetByteArray();
        public abstract NbtString GetString();
        public abstract NbtList<T> GetList<T>() where T : NbtTag;
        public abstract NbtCompound GetCompound();
        public abstract NbtIntArray GetIntArray();
        public abstract NbtLongArray GetLongArray();
        public static NbtTag Deserialize(Decoder decoder, byte typeid)
        {
            switch (typeid)
            {
                case 0:
                    {
                        return new NbtByte();
                    }
                case 1:
                    {
                        return new NbtByte(decoder);
                    }
                case 2:
                    {
                        return new NbtShort(decoder);
                    }
                case 3:
                    {
                        return new NbtInt(decoder);
                    }
                case 4:
                    {
                        return new NbtLong(decoder);
                    }
                case 5:
                    {
                        return new NbtFloat(decoder);
                    }
                case 6:
                    {
                        return new NbtDouble(decoder);
                    }
                case 7:
                    {
                        return new NbtByteArray(decoder);
                    }
                case 8:
                    {
                        return new NbtString(decoder);
                    }
                case 9:
                    {
                        byte type = decoder.ReadByte();
                        if (type == 1) return new NbtList<NbtByte>(decoder);
                        if (type == 2) return new NbtList<NbtShort>(decoder);
                        if (type == 3) return new NbtList<NbtInt>(decoder);
                        if (type == 4) return new NbtList<NbtLong>(decoder);
                        if (type == 5) return new NbtList<NbtFloat>(decoder);
                        if (type == 6) return new NbtList<NbtDouble>(decoder);
                        if (type == 7) return new NbtList<NbtByteArray>(decoder);
                        if (type == 8) return new NbtList<NbtString>(decoder);
                        if (type == 9) throw new NotSupportedException();
                        if (type == 10) return new NbtList<NbtCompound>(decoder);
                        if (type == 11) return new NbtList<NbtIntArray>(decoder);
                        if (type == 12) return new NbtList<NbtLongArray>(decoder);
                        throw new ArgumentOutOfRangeException();
                    }
                case 10:
                    {
                        return new NbtCompound(decoder);
                    }
                case 11:
                    {
                        return new NbtIntArray(decoder);
                    }
                case 12:
                    {
                        return new NbtLongArray(decoder);
                    }
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}

using Sakiy.Game.NBT;
using Sakiy.Type;

namespace Sakiy.Game.World
{
    public sealed class DimensionType : NbtCompound
    {
        public readonly Identifier ID;
        public readonly float AmbientLight; //TODO: does the client use this
        public readonly string Effects; //TODO: sky?
        public readonly long? FixedTime; //TODO: does the client use this
        public readonly bool HasSkylight;  //TODO: does the client use this
        public readonly int Height;  //TODO: does the client use this
        public readonly int MinY;  //TODO: does the client use this
        public readonly bool Natural;  //TODO: does the client use this
        /*public NbtCompound Nbt
        {
            get
            {
                Dictionary<string, NbtTag> element = new() {
                    { "ambient_light", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtFloat(AmbientLight) } }) },
                    { "bed_works", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(true) } }) },
                    { "coordinate_scale", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtDouble(1.0) } }) },
                    { "effects", new NbtString(Effects) },
                    { "has_ceiling", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(false) } }) },
                    { "has_raids", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(true) } }) },
                    { "has_skylight", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(HasSkylight) } }) },
                    { "height", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(Height) } }) },
                    { "infiniburn", new NbtString("#minecraft:overworld") },
                    { "logical_height", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(Height) } }) },
                    { "min_y", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(MinY) } }) },
                    { "monster_spawn_block_light_limit", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(0) } }) },
                    { "monster_spawn_light_level", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        { "type", new NbtString("minecraft:uniform") },
                        { "value", new NbtCompound(new Dictionary<string, NbtTag>()
                        {
                            { "max_inclusive", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(15) } }) },
                            { "min_inclusive", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(0) } }) }
                        })
                        },
                    }) },
                    { "natural", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(Natural) } }) },
                    { "piglin_safe", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(false) } }) },
                    { "respawn_anchor_works", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(true) } }) },
                    { "ultrawarm", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtByte(false) } }) },
                };
                if (FixedTime.HasValue) element.Add("fixed_time", new NbtLong(FixedTime.Value)); //TODO: fix (value)
                return new(element);
            }
        }*/
        public NbtCompound Nbt
        {
            get
            {
                Dictionary<string, NbtTag> element = new() {
                    { "ambient_light", new NbtFloat(AmbientLight) },
                    { "bed_works", new NbtByte(true) },
                    { "coordinate_scale", new NbtDouble(1.0) },
                    { "effects", new NbtString(Effects) },
                    { "has_ceiling", new NbtByte(false) },
                    { "has_raids", new NbtByte(true) },
                    { "has_skylight", new NbtByte(HasSkylight) },
                    { "height", new NbtInt(Height) },
                    { "infiniburn", new NbtString("#minecraft:overworld") },
                    { "logical_height", new NbtInt(Height) },
                    { "min_y", new NbtInt(MinY) },
                    { "monster_spawn_block_light_limit", new NbtInt(0) },
                    { "monster_spawn_light_level", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        { "type", new NbtString("minecraft:uniform") },
                        { "value", new NbtCompound(new Dictionary<string, NbtTag>()
                        {
                            { "max_inclusive", new NbtInt(15) },
                            { "min_inclusive", new NbtInt(0) }
                        })
                        },
                    }) },
                    { "natural", new NbtByte(Natural) },
                    { "piglin_safe", new NbtByte(false) },
                    { "respawn_anchor_works", new NbtByte(true) },
                    { "ultrawarm", new NbtByte(false) },
                };
                if (FixedTime.HasValue) element.Add("fixed_time", new NbtLong(FixedTime.Value));
                return new(element);
            }
        }

        public DimensionType(Identifier id, float ambientLight, string effects, long? fixedTime, bool hasSkylight, int height, int minY, bool natural)
        {
            ID = id;
            AmbientLight = ambientLight;
            Effects = effects;
            FixedTime = fixedTime;
            HasSkylight = hasSkylight;
            Height = height;
            MinY = minY;
            Natural = natural;
        }
    }
}

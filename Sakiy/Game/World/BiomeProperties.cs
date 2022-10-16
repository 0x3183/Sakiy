using Sakiy.Game.NBT;
using Sakiy.Type;

namespace Sakiy.Game.World
{
    public sealed class BiomeProperties
    {
        public sealed class MusicProperty
        {
            public readonly bool ReplaceCurrentMusic;
            public readonly Identifier Sound;
            public readonly int MaxDelay;
            public readonly int MinDelay;
            public MusicProperty(bool replaceCurrentMusic, Identifier sound, int maxDelay, int minDelay)
            {
                ReplaceCurrentMusic = replaceCurrentMusic;
                Sound = sound;
                MaxDelay = maxDelay;
                MinDelay = minDelay;
            }
        }
        public sealed class AdditionsSoundProperty
        {
            public readonly Identifier Sound;
            public readonly double TickChance;
            public AdditionsSoundProperty(Identifier sound, double tickChance)
            {
                Sound = sound;
                TickChance = tickChance;
            }
        }
        public sealed class MoodSoundProperty
        {
            public readonly Identifier Sound;
            public readonly int TickDelay;
            public readonly double Offset;
            public readonly int BlockSearchExtent;
            public MoodSoundProperty(Identifier sound, int tickDelay, double offset, int blockSearchExtent)
            {
                Sound = sound;
                TickDelay = tickDelay;
                Offset = offset;
                BlockSearchExtent = blockSearchExtent;
            }
        }
        public sealed class ParticleProperty
        {
            public readonly float Probability;
            public readonly Identifier Type;
            public ParticleProperty(float probability, Identifier type)
            {
                Probability = probability;
                Type = type;
            }
        }
        public readonly Identifier ID;
        public readonly string Precipitation;
        public readonly float? Depth;
        public readonly float Temperature;
        public readonly float? Scale;
        public readonly float Downfall;
        public readonly string? Category;
        public readonly string? TemperatureModifier;
        public readonly Color SkyColor;
        public readonly Color WaterFogColor;
        public readonly Color FogColor;
        public readonly Color WaterColor;
        public readonly Color? FoliageColor;
        public readonly Color? GrassColor;
        public readonly string? GrassColorModifier;
        public readonly MusicProperty? Music;
        public readonly string? AmbientSound;
        public readonly AdditionsSoundProperty? AdditionsSound;
        public readonly MoodSoundProperty? MoodSound;
        public readonly ParticleProperty? Particle;
        /*public NbtCompound Nbt
        {
            get
            {
                Dictionary<string, NbtTag> effects = new(12)
                {
                    { "sky_color", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(SkyColor.Value.GetHashCode()) } })},
                    { "water_fog_color", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(WaterFogColor.Value.GetHashCode()) } })},
                    { "fog_color", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(FogColor.Value.GetHashCode()) } })},
                    { "water_color", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(WaterColor.Value.GetHashCode()) } })}
                };
                if (FoliageColor.HasValue) effects.Add("foliage_color", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(FoliageColor.Value.Value.GetHashCode()) } }));
                if (GrassColor.HasValue) effects.Add("grass_color", new NbtCompound(new Dictionary<string, NbtTag>() { { "value", new NbtInt(GrassColor.Value.Value.GetHashCode()) } }));
                if (GrassColorModifier != null) effects.Add("grass_color_modifier", new NbtString(GrassColorModifier));
                if (Music != null) effects.Add("music", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"replace_current_music", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtByte(Music.ReplaceCurrentMusic)}
                    })},
                    {"sound", new NbtString(Music.Sound.ToString())},
                    {"max_delay", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtInt(Music.MaxDelay) }
                    })},
                    {"min_delay", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtInt(Music.MinDelay) }
                    })}
                }));
                if (AmbientSound != null) effects.Add("ambient_sound", new NbtString(AmbientSound));
                if (AdditionsSound != null) effects.Add("additions_sound", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"sound", new NbtString(AdditionsSound.Sound.ToString())},
                    {"tick_chance", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtDouble(AdditionsSound.TickChance)}
                    })}
                }));
                if (MoodSound != null) effects.Add("mood_sound", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"sound", new NbtString(MoodSound.Sound.ToString())},
                    {"tick_delay ", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtInt(MoodSound.TickDelay)}
                    })},
                    {"offset", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtDouble(MoodSound.Offset)}
                    })},
                    {"block_search_extent  ", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtInt(MoodSound.BlockSearchExtent)}
                    })}
                }));
                if (Particle != null) effects.Add("particle", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"probability", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"value", new NbtFloat(Particle.Probability)}
                    })},
                    {"options", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"type", new NbtString(Particle.Type.ToString())}
                    }) }
                }));
                Dictionary<string, NbtTag> element = new()
                {
                    { "precipitation", new NbtString(Precipitation) },
                    { "temperature", new NbtCompound(new Dictionary<string, NbtTag>(){ {"value", new NbtFloat(Temperature) } }) },
                    { "downfall", new NbtCompound(new Dictionary<string, NbtTag>(){ {"value", new NbtFloat(Downfall) } }) },
                    { "effects", new NbtCompound(effects) },
                };
                if (Depth.HasValue) element.Add("depth", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"value", new NbtFloat(Depth.Value)}
                }));
                if (Scale.HasValue) element.Add("scale", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"value", new NbtFloat(Scale.Value)}
                }));
                if (Category != null) element.Add("category", new NbtString(Category));
                if (TemperatureModifier != null) element.Add("temperature_modifier", new NbtString(TemperatureModifier));
                return new(element);
            }
        }*/
        public NbtCompound Nbt
        {
            get
            {
                Dictionary<string, NbtTag> effects = new(12)
                {
                    { "sky_color", new NbtInt(SkyColor.Value.GetHashCode()) },
                    { "water_fog_color", new NbtInt(WaterFogColor.Value.GetHashCode()) },
                    { "fog_color", new NbtInt(FogColor.Value.GetHashCode()) },
                    { "water_color", new NbtInt(WaterColor.Value.GetHashCode()) }
                };
                if (FoliageColor.HasValue) effects.Add("foliage_color", new NbtInt(FoliageColor.Value.Value.GetHashCode()));
                if (GrassColor.HasValue) effects.Add("grass_color", new NbtInt(GrassColor.Value.Value.GetHashCode()));
                if (GrassColorModifier != null) effects.Add("grass_color_modifier", new NbtString(GrassColorModifier));
                if (Music != null) effects.Add("music", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"replace_current_music", new NbtByte(Music.ReplaceCurrentMusic) },
                    {"sound", new NbtString(Music.Sound.ToString()) },
                    {"max_delay", new NbtInt(Music.MaxDelay) },
                    {"min_delay", new NbtInt(Music.MinDelay) }
                }));
                if (AmbientSound != null) effects.Add("ambient_sound", new NbtString(AmbientSound));
                if (AdditionsSound != null) effects.Add("additions_sound", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"sound", new NbtString(AdditionsSound.Sound.ToString()) },
                    {"tick_chance", new NbtDouble(AdditionsSound.TickChance) }
                }));
                if (MoodSound != null) effects.Add("mood_sound", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"sound", new NbtString(MoodSound.Sound.ToString())},
                    {"tick_delay ", new NbtInt(MoodSound.TickDelay) },
                    {"offset", new NbtDouble(MoodSound.Offset) },
                    {"block_search_extent  ", new NbtInt(MoodSound.BlockSearchExtent) }
                }));
                if (Particle != null) effects.Add("particle", new NbtCompound(new Dictionary<string, NbtTag>()
                {
                    {"probability", new NbtFloat(Particle.Probability) },
                    {"options", new NbtCompound(new Dictionary<string, NbtTag>()
                    {
                        {"type", new NbtString(Particle.Type.ToString())}
                    }) }
                }));
                Dictionary<string, NbtTag> element = new()
                {
                    { "precipitation", new NbtString(Precipitation) },
                    { "temperature", new NbtFloat(Temperature) },
                    { "downfall", new NbtFloat(Downfall) },
                    { "effects", new NbtCompound(effects) },
                };
                if (Depth.HasValue) element.Add("depth", new NbtFloat(Depth.Value));
                if (Scale.HasValue) element.Add("scale", new NbtFloat(Scale.Value));
                if (Category != null) element.Add("category", new NbtString(Category));
                if (TemperatureModifier != null) element.Add("temperature_modifier", new NbtString(TemperatureModifier));
                return new(element);
            }
        }
        public BiomeProperties(Identifier id, string precipitation, float? depth, float temperature, float? scale, float downfall, string? category, string? temperatureModifier, Color skyColor, Color waterFogColor, Color fogColor, Color waterColor, Color? foliageColor, Color? grassColor, string? grassColorModifier, MusicProperty? music, string? ambientSound, AdditionsSoundProperty? additionsSound, MoodSoundProperty? moodSound, ParticleProperty? particle)
        {
            ID = id;
            Precipitation = precipitation;
            Depth = depth;
            Temperature = temperature;
            Scale = scale;
            Downfall = downfall;
            Category = category;
            TemperatureModifier = temperatureModifier;
            SkyColor = skyColor;
            WaterFogColor = waterFogColor;
            FogColor = fogColor;
            WaterColor = waterColor;
            FoliageColor = foliageColor;
            GrassColor = grassColor;
            GrassColorModifier = grassColorModifier;
            Music = music;
            AmbientSound = ambientSound;
            AdditionsSound = additionsSound;
            MoodSound = moodSound;
            Particle = particle;
        }
    }
}

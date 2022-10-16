using Sakiy.Type;

namespace Sakiy.Game.World
{
    public static class World
    {
        public static readonly List<Dimension> Dimensions = new();
        public static readonly List<BiomeProperties> Biomes = new();
        public static readonly List<DimensionType> DimensionTypes = new();
        public static readonly List<Player> Players = new();
        public static bool Hardcore { get; set; } = false;
        public static bool Flat { get; set; } = false;
        public static bool ReducedDebugInfo { get; set; } = false;
        public static bool EnableRespawnScreen { get; set; } = true;
        public static int ViewDistance { get; set; } = 32;
        public static int SimulationDistance { get; set; } = 32;

        public static bool Empty { 
            
            get
            {
                return !Players.Any();
            } 
        }
    }
}

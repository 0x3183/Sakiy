using Sakiy.Game.World.Blocks;
using Sakiy.Game.World.Blocks.Enums;

namespace Sakiy.Game.World
{
    public abstract class Block
    {
        public static readonly Dictionary<int, Block> BlocksByID = new()
        {
            { 0, new Air() },
            { 1, new Stone() },
        };
        public static readonly Dictionary<Block, int> IDByBlocks = new()
        {
            { new Air(), 0 },
            { new Stone(), 1 },
        };
    }
}

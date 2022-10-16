using Sakiy.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakiy.Game.World
{
    public sealed class Dimension
    {
        public readonly List<Entity> Entities = new();
        public readonly Identifier ID;
        public readonly DimensionType Type;
        public Dimension(Identifier id, DimensionType type)
        {
            ID = id;
            Type = type;
        }
        //entities and players
        //chunks

        //dimension type codec somehow

        //dimension name Identifier
        //
    }
}

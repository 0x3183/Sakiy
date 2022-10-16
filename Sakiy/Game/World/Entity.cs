using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakiy.Game.World
{
    public abstract class Entity
    {
        public readonly int EID;
        public Entity(int eid)
        {
            EID = eid;
        }
    }
}

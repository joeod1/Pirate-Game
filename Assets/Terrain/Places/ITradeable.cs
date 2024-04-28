using Assets.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Terrain.Places
{
    public interface ITradeable
    {
        public void Trade(Ship ship, TradeDeal deal);

        public void InRadius(Ship ship);
    }
}

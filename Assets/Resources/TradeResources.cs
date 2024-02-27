using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class TradeResources
    {
        int wood, food, oranges, gold,
            water, drink;
        public Dictionary<CannonBallType, int> cannonballs =
        new Dictionary<CannonBallType, int>(){
            { CannonBallType.Standard, 0 },
            { CannonBallType.Grapeshot, 0 },
            { CannonBallType.Incendiary, 0 },
            { CannonBallType.ChainShot, 0 },
            { CannonBallType.Cursed, 0 },
            { CannonBallType.WaterBalloon, 0 }
        };
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public enum ResourceType : ushort
    {
        Wood, Food, Oranges, Gold, Water, Drink, CannonBalls
    }

    public class TradeResources
    {
        public Dictionary<ResourceType, int> quantities = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 1000 },
            { ResourceType.Food, 0 },
            { ResourceType.Oranges, 0 },
            { ResourceType.Gold, 0 },
            { ResourceType.Water, 0 },
            { ResourceType.Drink, 0 },
            { ResourceType.CannonBalls, 0 },
        };

        public Dictionary<CannonBallType, int> cannonballs =
        new Dictionary<CannonBallType, int>(){
            { CannonBallType.Standard, 0 },
            { CannonBallType.Grapeshot, 0 },
            { CannonBallType.Incendiary, 0 },
            { CannonBallType.ChainShot, 0 },
            { CannonBallType.Cursed, 0 },
            { CannonBallType.WaterBalloon, 0 }
        };

        static public Dictionary<ResourceType, int> weights = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 8 },
            { ResourceType.Food, 5 },
            { ResourceType.Oranges, 3 },
            { ResourceType.Gold, 10 },
            { ResourceType.Water, 8 },
            { ResourceType.Drink, 4 },
            { ResourceType.CannonBalls, 4 },
        };

        static public int QuantityFromWeight(ResourceType type, int weight)
        {
            return weight / weights[type];
        }

        static public int WeightFromQuantity(ResourceType type, int quantity)
        {
            return weights[type] * quantity;
        }

        public int GetWeight()
        {
            int weight = 0;
            foreach (KeyValuePair<ResourceType, int> quantity in quantities)
            {
                weight += WeightFromQuantity(quantity.Key, quantity.Value);
            }
            return weight;
        }
    }
}

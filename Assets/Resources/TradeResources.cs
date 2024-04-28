using Assets.Logic;
using System;
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

    public delegate bool ForResourceQuantity(ResourceType type, int ct);


    [Serializable]
    public class TradeResources
    {
        [SerializeField]
        public SerializableDict<ResourceType, float> quantities = new SerializableDict<ResourceType, float>(
            new Dictionary<ResourceType, float> {
                { ResourceType.Wood, 100 },
                { ResourceType.Food, 100 },
                { ResourceType.Oranges, 100 },
                { ResourceType.Gold, 100 },
                { ResourceType.Water, 100 },
                { ResourceType.Drink, 100 },
                { ResourceType.CannonBalls, 100 },
            }
        );

        [SerializeField]
        public SerializableDict<CannonBallType, int> cannonballs = new SerializableDict<CannonBallType, int>(
            new Dictionary<CannonBallType, int> {
                { CannonBallType.Standard, 0 },
                { CannonBallType.Grapeshot, 0 },
                { CannonBallType.Incendiary, 0 },
                { CannonBallType.ChainShot, 0 },
                { CannonBallType.Cursed, 0 },
                { CannonBallType.WaterBalloon, 0 }
            }
        );

        [SerializeField]
        static public SerializableDict<ResourceType, int> weights = new SerializableDict<ResourceType, int>(
            new Dictionary<ResourceType, int> {
                { ResourceType.Wood, 8 },
                { ResourceType.Food, 5 },
                { ResourceType.Oranges, 3 },
                { ResourceType.Gold, 10 },
                { ResourceType.Water, 8 },
                { ResourceType.Drink, 4 },
                { ResourceType.CannonBalls, 4 },
            }
        );

        static public int QuantityFromWeight(ResourceType type, int weight)
        {
            return weight / weights[type];
        }

        static public float WeightFromQuantity(ResourceType type, float quantity)
        {
            return weights[type] * quantity;
        }

        public float GetWeight()
        {
            float weight = 0;
            foreach (KeyValuePair<ResourceType, float> quantity in quantities)
            {
                weight += WeightFromQuantity(quantity.Key, quantity.Value);
            }
            return weight;
        }

        public static TradeResources operator+(TradeResources one, TradeResources two)
        {
            TradeResources result = new TradeResources();

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                result.quantities[type] = 0;
                if (one.quantities.ContainsKey(type)) result.quantities[type] += one.quantities[type];
                if (two.quantities.ContainsKey(type)) result.quantities[type] += two.quantities[type];
            }

            return result;
        }

        public static TradeResources operator -(TradeResources one, TradeResources two)
        {
            TradeResources result = new TradeResources();

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                result.quantities[type] = 0;
                if (one.quantities.ContainsKey(type)) result.quantities[type] += one.quantities[type];
                if (two.quantities.ContainsKey(type)) result.quantities[type] -= two.quantities[type];
            }

            return result;
        }

        public new string ToString()
        {
            string resourceString = "";
            if (quantities[ResourceType.Wood] > 0)
            {
                resourceString += "Wood: " + quantities[ResourceType.Wood] + "\n";
            }
            if (quantities[ResourceType.Food] > 0)
            {
                resourceString += "Food: " + quantities[ResourceType.Food] + "\n";
            }
            if (quantities[ResourceType.Oranges] > 0)
            {
                resourceString += "Oranges: " + quantities[ResourceType.Oranges] + "\n";
            }
            if (quantities[ResourceType.Water] > 0)
            {
                resourceString += "Water: " + quantities[ResourceType.Water] + "\n";
            }
            if (quantities[ResourceType.Drink] > 0)
            {
                resourceString += "Drink: " + quantities[ResourceType.Drink] + "\n";
            }
            if (quantities[ResourceType.CannonBalls] > 0)
            {
                resourceString += "CannonBalls: " + quantities[ResourceType.CannonBalls] + "\n";
            }
            return resourceString;
        }
    }
}

using Assets.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Logic
{
    public class Market : MonoBehaviour
    {
        public PlayerShipController1 playerShip;
        public static Market Instance;

        private void Start()
        {
            Instance = this;
        }

        private void Awake()
        {
            Instance = this;
        }

        public static bool Transact(ResourceType takeType, int takeQt, ResourceType exchangeType, int exchangeQt)
        {
            TradeResources cargo = Instance.playerShip.ship.cargo;
            if (cargo.quantities[exchangeType] < exchangeQt) return false;

            cargo.quantities[takeType] += takeQt;
            cargo.quantities[exchangeType] -= exchangeQt;

            return true;
        }

        public static bool GiveCannons(ResourceType exchangeType, int exchangeQt)
        {
            TradeResources cargo = Instance.playerShip.ship.cargo;
            if (cargo.quantities[exchangeType] < exchangeQt) return false;

            Instance.playerShip.ship.cannonCount += 2;
            Instance.playerShip.ship.PlaceCannons();

            cargo.quantities[exchangeType] -= exchangeQt;
            return true;
        }
    }
}

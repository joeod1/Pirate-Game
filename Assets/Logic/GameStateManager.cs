using Assets.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Logic
{
    public class GameStateManager : MonoBehaviour
    {
        public PlayerShipController1 playerShip;
        public Character playerCharacter;

        public ShipSideGenerator sideGenerator;
        public ActiveMapPlacer topdownGenerator;

        
        private void Start()
        {
            SystemsManager.BoardShip = LoadShip;
            SystemsManager.DockPort = LoadPort;
        }

        public void LoadPort(Port target, Ship sender)
        {

        }

        public void LoadShip(Ship target, Ship sender)
        {
            sideGenerator.playerShip = sender;
            sideGenerator.GenerateShip(target);
            topdownGenerator.gameObject.SetActive(false);
            sideGenerator.gameObject.SetActive(true);
        }

        public void LeaveShip()
        {
            topdownGenerator.gameObject.SetActive(true);
            sideGenerator.gameObject.SetActive(false);
        }
    }
}

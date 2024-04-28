using Assets.PlatformerFolder;
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
            // PlatformerManager.Instance.portGenerator.
            if (PlatformerManager.Instance == null)
            {
                PlatformerManager.Instance = sideGenerator.GetComponent<PlatformerManager>();
                PlatformerManager.Instance.Awake();
            }
            PlatformerManager.Instance.ResetTerrain();
            PlatformerManager.Instance.portGenerator.coGeneratePort(target);
            
            topdownGenerator.gameObject.SetActive(false);
            PlatformerManager.Instance.gameObject.SetActive(true);
        }

        public void LoadShip(Ship target, Ship sender)
        {
            if (PlatformerManager.Instance == null)
            {
                PlatformerManager.Instance = sideGenerator.GetComponent<PlatformerManager>();
                PlatformerManager.Instance.Awake();
            }
            PlatformerManager.Instance.ResetTerrain();
            sideGenerator.playerShip = sender;
            sideGenerator.ship = target;
            topdownGenerator.gameObject.SetActive(false);
            sideGenerator.gameObject.SetActive(true);
            sideGenerator.GenerateShip(target);
        }

        public void LeaveShip()
        {
            topdownGenerator.gameObject.SetActive(true);
            sideGenerator.gameObject.SetActive(false);
        }
    }
}

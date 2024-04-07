using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Assets.Ships;
using Unity.VisualScripting.FullSerializer;

namespace Assets.Logic
{
    [Serializable]
    public class SaveStateManager
    {
        public string saveloc = "./saves/";

        [SerializeField]
        public SerializeType<EnemyShipController1> enemies;
        [SerializeField] public SerializeType<PlayerShipController1> player;
        [SerializeField]
        public SerializeType<Port> ports;
        //public List<string> enemyShips;
        //public List<string> playerShip;

        /*public void SerializeToList(var list, out List<string> output)
        {

        }*/

        public bool SaveGame(string savename = "save")
        {
            PopulateLists();
            string savestring = JsonUtility.ToJson(this, true);
            File.WriteAllText(saveloc + savename + ".sav", savestring);
            return false;
        }

        public void PopulateLists()
        {
            enemies.GetElements();
            player.GetElements();
            Debug.Log("Player objects: " + player.objects.Count);
            ports.GetElements();
        }
    }
}

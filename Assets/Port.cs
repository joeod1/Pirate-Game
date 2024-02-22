using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Port : MonoBehaviour
    {
        [Header("Map")]
        public TerrainGeneration terrainGenerator;
        public Vector3Int cell;
        public Vector3Int dockCell;

        [Header("Relations")]
        public bool pirateFriendly = false;
        public Dictionary<string, float> pirateRelations;

        [Header("Wealth")]
        public float wealth;
        public float food;
        public float drink;
        public float gold;
        public int production; // change to enum; produces two of [wood, food, drink, gold]

        [Header("Ships")]
        public List<EnemyShipController> ships;

        // what if wood is produced by chopping down wood cells?
        // create a dictionary and iterate through the cells rendered in terraingenerator

        // possibly create a new class that identify production time remaining, etc
        // how do we cut down trees without the terrain generator working? should each Port be a gameobject? probably
        public Port(Vector3Int inputCell, bool friendly = false)
        {
            cell = inputCell;
            pirateFriendly = friendly;
        }
    }
}

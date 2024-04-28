using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets.PlatformerFolder
{
    [Serializable]
    public class PlatformerPalette
    {
        [Header("Output Locations")]
        public GameObject partsContainer;
        public GameObject characterContainer;
        public Tilemap tilemap;
        public Tilemap groundTilemap;

        [Header("Common Instances")]
        public Light sunLighting;
        public GameObject water;

        [Header("Entity Prefabs")]
        public GameObject npcPrefab;
        public GameObject playerPrefab;

        [Header("Part Prefabs")]
        public GameObject platform;
        public GameObject ladder;
        public GameObject wall;
        public GameObject beam;
        public GameObject lamp;
        public GameObject weaponStand;
        public GameObject foodStand;
        public GameObject playerCount;
        public GameObject dock;

        [Header("Background Tiles")]
        public Tile woodTile;
        public Tile groundTop;
        public Tile groundMiddle;
    }
}

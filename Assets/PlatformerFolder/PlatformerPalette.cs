using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Assets.PlatformerFolder
{
    [Serializable]
    public class PlatformerPalette
    {
        [Header("Output Locations")]
        public GameObject partsContainer;
        public GameObject characterContainer;
        public Tilemap tilemap;

        [Header("Entity Prefabs")]
        public GameObject npcPrefab;
        public GameObject playerPrefab;

        [Header("Part Prefabs")]
        public GameObject platform;
        public GameObject ladder;
        public GameObject wall;
        public GameObject beam;
        public GameObject lamp;

        [Header("Background Tiles")]
        public Tile woodTile;
    }
}

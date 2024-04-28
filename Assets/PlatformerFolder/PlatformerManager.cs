using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.PlatformerFolder
{
    public class PlatformerManager : MonoBehaviour
    {
        public static PlatformerManager Instance { get; set; }

        [Header("Level Generators")]
        public ShipSideGenerator shipGenerator;
        public PortSideGenerator portGenerator;

        [Header("Palette")]
        public PlatformerPalette palette = new PlatformerPalette();

        public void Awake()
        {
            Instance = this;
            portGenerator.palette = palette;
            shipGenerator.palette = palette;
        }

        public void Start()
        {
            Instance = this;
            portGenerator.palette = palette;
            shipGenerator.palette = palette;
        }

        public void ResetTerrain()
        {
            for (int i = 0; i < palette.partsContainer.transform.childCount; i++)
            {
                Destroy(palette.partsContainer.transform.GetChild(i).gameObject);
            }
            palette.groundTilemap.ClearAllTiles();
            palette.tilemap.ClearAllTiles();
        }
    }
}

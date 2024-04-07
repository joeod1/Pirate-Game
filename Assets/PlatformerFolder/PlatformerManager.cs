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
        public static PlatformerManager Instance { get; private set; }

        [Header("Level Generators")]
        public ShipSideGenerator shipGenerator;
        public PortSideGenerator portGenerator;

        [Header("Palette")]
        public PlatformerPalette palette = new PlatformerPalette();

        private void Awake()
        {
            Instance = this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Terrain
{
    public class TerrainZone
    {
        public Vector2 position;
        public Vector2Int size;

        public TerrainZone(Vector2 position, Vector2Int size)
        {
            Set(position, size);
        }

        public void Set(Vector2 position, Vector2Int size)
        {
            this.position = position;
            this.size = size;
        }
    }
}

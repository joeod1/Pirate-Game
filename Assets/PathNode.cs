using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class PathNode
    {
        public PathNode() { }
        public PathNode(Vector3Int icell) { cell = icell; }

        public float f = 0;
        public float g = 0;
        public float h = 0;
        public float depth;
        public Vector2 position;
        public Vector3Int cell;
        public PathNode prior;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Path
    {
        public Path() { }

        public Vector2 start, end;
        public float distance;
        public PathNode currentNode;

        public PathNode NextTarget()
        {
            PathNode tmp = currentNode;
            currentNode = currentNode.prior;
            return tmp;
        }
        public Vector2 NextPosition()
        {
            return NextTarget().position;
        }
    }
}

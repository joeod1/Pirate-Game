using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class Path : ISerializationCallbackReceiver
    {
        public Path() { }

        public Vector2 start, end;
        public float distance;
        public PathNode startNode;
        public PathNode currentNode;

        public List<PathNode> nodes = new List<PathNode>();

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

        public void UpdateArray()
        {
            nodes.Clear();
            PathNode tmp = currentNode;
            int ct = 0;
            while (tmp != null && ct < 1000)
            {
                ct++;
                nodes.Add(tmp);
                tmp = tmp.prior;
            }
        }

        public void OnBeforeSerialize()
        {
            // UpdateArray();
        }

        public void OnAfterDeserialize()
        {
            if (nodes.Count == 0) return;
            currentNode = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                currentNode.prior = nodes[i];
                currentNode = currentNode.prior;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class PathNode : ISerializationCallbackReceiver
    {
        public PathNode() { }
        public PathNode(Vector3Int icell) { cell = icell; }

        public float f = 0;
        public float g = 0;
        public float h = 0;
        public float depth;
        public Vector2 position;
        public Vector3Int cell;

        [DoNotSerialize]
        public PathNode prior;

        public void OnBeforeSerialize()
        {
            /*next.Clear();
            PathNode tmp = this;
            int ct = 0;
            while (tmp != null && ct++ < 100)
            {
                tmp = tmp.prior;
                next.Add(tmp);
            }*/
        }

        public void OnAfterDeserialize()
        {
            /*PathNode tmp = this;
            for (int i = 0; i < next.Count; i++)
            {
                tmp.prior = next[i];
                tmp = tmp.prior;
            }*/
        }
    }
}

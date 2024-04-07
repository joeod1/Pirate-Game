using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.PlatformerFolder
{
    [Serializable]
    public class LadderMap
    {
        Dictionary<int, float> ladderPosition = new Dictionary<int, float>();
        float floorHeight = 3;

        public float Get(float height)
        {
            float value;
            if (ladderPosition.TryGetValue((int)(height / floorHeight), out value))
            {
                return value;
            }
            return -1f;
        }

        public void Set(UnityEngine.Vector3 position)
        {
            ladderPosition.Add(
                (int)(position.y / floorHeight),
                position.x
                );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Config : MonoBehaviour
    {
        [Header("World Generation")]
        public Vector2Int bounds = new Vector2Int(100, 100);
        public float seed = -1;
        public int numPorts = 25;
        public float waterLevel = 0.15f;
        public float featureSize = 7f;
        public float featureSizeVariation = 1.0f;
        public float featureSizeVariationFrequency = 7f;
        public float biomeSize = 32f;

        [Header("Difficulty")]
        static public int difficulty = 0;

        public void Start()
        {
            if (seed == -1) seed = UnityEngine.Random.Range(0f, 1000f);
        }
    }
}

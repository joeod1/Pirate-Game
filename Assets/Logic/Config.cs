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
        public Vector2Int bounds = new Vector2Int(75, 75);//new Vector2Int(100, 100);
        public float seed = -1;
        public int numPorts = 25;
        public float waterLevel = 0.4f; //0.15f;
        public float featureSize = 15f; //7f;
        public float featureSizeVariation = 0.1f;  //1.0f;
        public float featureSizeVariationFrequency = 8f;  //7f;
        public float biomeSize = 32f;

        [Header("Difficulty")]
        public int difficulty = 0;

        public void Start()
        {
            if (seed == -1) seed = (int)UnityEngine.Random.Range(0f, 100000f);
        }
    }
}

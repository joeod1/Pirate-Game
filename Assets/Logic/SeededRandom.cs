using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;

namespace Assets.Logic
{
    public class SeededRandom
    {
        public static long seed = 0;

        public static long Seed(long seed, bool set = true)
        {
            if (set) SeededRandom.seed = seed;
            return seed;
        }

        public static long Seed(string seed, bool set = true)
        {
            long tmp = String2Seed(seed);
            if (set) SeededRandom.seed = tmp;
            return tmp;
        }

        public static long String2Seed(string seed)
        {
            long tmp = 0;
            for (int i = 0; i < seed.Length; i++)
            {
                tmp += (int)seed.ElementAt<char>(i);
            }
            return tmp;
        }

        public static float RangeFloat(float2 pos, float min = 0, float max = 1, long offsetSeed = 0)
        {
            float range = max - min;
            return (range + noise.snoise(1000 * new float2(pos.x + offsetSeed, pos.y - offsetSeed)) * range ) % range  + min;
        }

        public static int RangeInt(float2 pos, float min = 0, float max = 1, long offsetSeed = 0)
        {
            return (int)RangeFloat(pos, min, max, offsetSeed);
        }
    }
}

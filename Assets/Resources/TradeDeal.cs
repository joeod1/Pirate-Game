using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Resources
{
    [Serializable]
    public class TradeDeal
    {
        public ResourceType exchange;
        public int exchangeQt;
        public GameObject exchangePt;

        public ResourceType take;
        public int takeQt;
        public GameObject homePt;

        public bool finished = false;
    }
}

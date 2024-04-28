using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Ships
{
    public interface ICanLoot
    {
        public void PromptContainer(GameObject obj);
        public void LeaveContainer(GameObject obj);
    }
}

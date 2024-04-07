using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.PlatformerFolder.Assets
{
    public class SideController : MonoBehaviour
    {
        protected Character character;

        public virtual void Start()
        {
            character = GetComponent<Character>();
        }
    }
}

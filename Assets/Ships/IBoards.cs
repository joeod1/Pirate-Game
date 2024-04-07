using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Ships
{
    public interface IBoards
    {
        public void EnteredRadius(GameObject other);
        public void LeftRadius(GameObject other);
    }
}

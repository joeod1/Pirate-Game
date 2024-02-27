using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Cannon : MonoBehaviour
    {
        public bool primed = false;
        public CannonBallType loadedType;
        private IEnumerator cor;

        public void BeginLoad(int loadTime = 10)
        {
            if (primed) return;
            cor = Load(loadTime);
            StartCoroutine(cor);
        }

        private IEnumerator Load(int time)
        {
            while (!primed)
            {
                yield return new WaitForSeconds(time);
                primed = true;
            }
        }

        public void Fire()
        {

        }
    }
}

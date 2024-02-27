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
        public Vector2 speed = Vector2.one;
        public CannonBallType loadedType;
        public GameObject CannonBallPrefab;
        public GameObject projectileContainer;
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
            print("Firing cannon!");
            GameObject projectile = Instantiate(CannonBallPrefab);
            projectile.transform.position = this.transform.position;
            Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
            rb2D.AddForce(this.transform.up * speed);
        }
    }
}

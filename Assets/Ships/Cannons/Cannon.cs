using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace Assets
{
    public class Cannon : MonoBehaviour
    {
        int loadSem = 1;
        public bool primed = true;
        public Vector2 speed = Vector2.one;
        public CannonBallType loadedType;
        public GameObject CannonBallPrefab;
        public GameObject projectileContainer;
        private IEnumerator cor;

        public void BeginLoad(int loadTime = 1)
        {
            if (primed || loadSem == 0) return;
            cor = Load(loadTime);
            StartCoroutine(cor);
        }

        private IEnumerator Load(int time)
        {
            int mine = 0;
            int possession = Interlocked.Exchange(ref loadSem, mine);
            while (!primed && possession > 0)
            {
                yield return new WaitForSeconds(time);
                primed = true;
            }
            loadSem++;
        }

        public void Fire()
        {
            if (!primed)
            {
                BeginLoad();
                return;
            }
            print("Firing cannon!");
            GameObject projectile = Instantiate(CannonBallPrefab);
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>());
            projectile.SetActive(true);
            projectile.transform.position = this.transform.position;
            Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
            rb2D.AddForce(this.transform.up * speed);
            CannonBall projectileController = projectile.GetComponent<CannonBall>();
            projectileController.parent = this.GetComponentInParent<ShipController>();
            primed = false;
            BeginLoad();
        }
    }
}

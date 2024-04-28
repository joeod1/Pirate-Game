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
        public GameObject fireSound;
        public GameObject parent;
        private IEnumerator cor;
        public bool selfManaging = true;

        public ProjectileHit LandedCallback;

        public AudioClip cannonSound;

        public ForResourceQuantity ConsumeResource;

        private void Awake()
        {
            primed = true;
            loadSem = 1;
        }

        private void OnEnable()
        {
            loadSem = 1;
        }

        public void BeginLoad(int loadTime = 1)
        {
            if (!selfManaging) return;
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
                if (ConsumeResource != null)
                {
                    if (!ConsumeResource(ResourceType.CannonBalls, 1))
                    {
                        break;
                    }
                }

                yield return new WaitForSeconds(time);
                primed = true;
            }
            loadSem++;
        }

        public void Fire()
        {
            if (!primed && selfManaging)
            {
                BeginLoad();
                return;
            }
            print("Firing cannon!");
            GetComponent<AudioSource>().PlayOneShot(cannonSound);

            GameObject projectile = Instantiate(CannonBallPrefab);

            Collider2D parentCollider = GetComponentInParent<Collider2D>();
            if (parentCollider != null)
                Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>());   

            projectile.SetActive(true);
            projectile.transform.position = this.transform.position;
            Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
            rb2D.AddForce(this.transform.up * speed);
            CannonBall projectileController = projectile.GetComponent<CannonBall>();
            projectileController.parent = parent;  // OnCollision = LandedCallback;
            // projectileController.parent = this.GetComponentInParent<ShipController>();
            primed = false;
            BeginLoad();
        }
    }
}

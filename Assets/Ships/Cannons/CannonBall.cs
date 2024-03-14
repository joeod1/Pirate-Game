using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Assets
{
    public enum CannonBallType : ushort
    {
        Standard,
        Grapeshot,
        Incendiary,
        ChainShot,
        Cursed,
        WaterBalloon
    }

    public class CannonBall : MonoBehaviour
    {
        public float damage = 10;
        public ShipController parent;
        private Rigidbody2D rb2D;

        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ShipController controller = collision.GetComponentInParent<ShipController>();
            if (controller != null)
                if (controller != parent)
                {
                    controller.DamageShip(damage);
                    // controller.health -= damage;
                    // float upDir = transform.up;
                    // rb2D.rotation
                    float cannonRot = math.atan2(rb2D.velocity.x, rb2D.velocity.y);
                    print(cannonRot);
                    float shipRot = collision.transform.rotation.z; //controller.transform.rotation.z;
                    float offRot = cannonRot - shipRot;
                    // print(offRot);
                    controller.offAxis += new Vector2(math.sin(cannonRot) * -10, math.cos(cannonRot) * 10);//45f * math.sin((offRot + 90) / 57.23f), 45f * math.sin((offRot + 180) / 57.23f));
                }
                else return;

            Port port = collision.GetComponent<Port>();
            if (port != null) return;
            
            Destroy(this.gameObject);
        }
    }
}

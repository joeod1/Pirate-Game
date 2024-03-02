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
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ShipController controller = collision.GetComponentInParent<ShipController>();
            if (controller != null)
                if (controller != parent)
                {
                    controller.health -= damage;

                    float cannonRot = transform.rotation.eulerAngles.z;
                    float shipRot = controller.transform.rotation.eulerAngles.z;
                    float offRot = ((cannonRot - shipRot + 360) % 360);
                    controller.offAxis += new Vector2(math.sin(offRot) * 30, math.cos(offRot) * 30);//45f * math.sin((offRot + 90) / 57.23f), 45f * math.sin((offRot + 180) / 57.23f));
                }
                else return;

            Port port = collision.GetComponent<Port>();
            if (port != null) return;
            
            Destroy(this.gameObject);
        }
    }
}

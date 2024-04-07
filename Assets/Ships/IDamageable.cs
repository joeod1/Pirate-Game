using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Ships
{
    public interface IDamageable
    {
        public void Damage(float dmg, GameObject attacker);
        public void Heal(float dmg, GameObject healer);
        public float GetHealth();
        public float GetMaxHealth();
    }
}

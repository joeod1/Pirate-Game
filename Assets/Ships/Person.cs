using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Ships
{
    public class Person : MonoBehaviour
    {
        [Header("Superficial")]
        public string name = "Name";

        [Header("Stats")]
        public float health = 100;
        public float maxHealth = 100;

        [Header("Characteristics")]
        public bool playerFriendly = true;
        public bool pacifist = true;
        public bool hasSword = false;
        public bool hasFlintlock = false;
    }
}

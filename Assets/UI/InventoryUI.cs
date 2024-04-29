using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Search;
using UnityEngine;

namespace Assets.UI
{
    public class InventoryUI : MonoBehaviour
    {
        static public InventoryUI Instance;
        public TMPro.TMP_Text woodAmount;
        public TMPro.TMP_Text cannonballsAmount;
        public TMPro.TMP_Text foodAmount;
        public TMPro.TMP_Text orangesAmount;
        public TMPro.TMP_Text waterAmount;
        public TMPro.TMP_Text drinkAmount;
        public TMPro.TMP_Text goldAmount;

        private void Start()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) ToggleVisible();
        }

        static public void ToggleVisible()
        {
            if (Instance.gameObject.activeSelf)
            {
                Instance.gameObject.SetActive(false);
            } else
            {
                Instance.gameObject.SetActive(true);
            }
        }
    }
}

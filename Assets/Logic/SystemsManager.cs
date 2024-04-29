using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Logic
{
    public delegate void ShipShip(Ship target, Ship caller);
    public delegate void PortShip(Port target, Ship caller);

    public class SystemsManager : MonoBehaviour
    {
        public static ShipShip BoardShip;
        public static PortShip DockPort;

        // An reference to the singleton
        public static SystemsManager Instance { get; private set; }

        
        public GameObject newGameScreen;
        public SaveStateManager saveStateManager = new();

        public TerrainGeneration terrainGenerator;

        public Image blackScreen;

        public LoadingBar loadingBar;
        public TMPro.TMP_Text hint;

        public Config iConfig;
        public static Config Config { get { return Instance.iConfig; } }

        public void Start()
        {
            newGameScreen.SetActive(true);
        }

        public static void SetHint(string text)
        {
            Instance.hint.text = text;
        }

        public static void UnsetHint(string text)
        {
            if (Instance.hint.text == text)
                Instance.hint.text = "";
        }

        public IEnumerator coFadeBlack()
        {
            float currentFade = 0f;
            while (currentFade < 1f)
            {
                currentFade += Time.deltaTime;
                blackScreen.color = new Color(0, 0, 0, currentFade);
                yield return null;
            }
        }

        public static void FadeToBlack()
        {
            Instance.blackScreen.gameObject.SetActive(true);
            Instance.StartCoroutine(Instance.coFadeBlack());
        }

        public IEnumerator coFadeIn()
        {
            float currentFade = 1f;
            while (currentFade > 0f)
            {
                currentFade -= Time.deltaTime;
                blackScreen.color = new Color(0, 0, 0, currentFade);
                yield return null;
            }
            Instance.blackScreen.gameObject.SetActive(false);
        }

        public static void FadeFromBlack()
        {
            Instance.StartCoroutine(Instance.coFadeIn());
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}

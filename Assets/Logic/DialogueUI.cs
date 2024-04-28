using Ink.Runtime;
using sfw.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Logic
{
    public class DialogueUI : MonoBehaviour
    {
        public static DialogueUI Instance { get; private set; }
        public RectTransform panel;
        public UnityEngine.UI.Button dialogueOptionPrefab;

        public UnityEvent<Choice> choiceSelection;

        private void Awake()
        {
            Instance = this;
        }

        public void PopulateChoices(List<Choice> choices)
        {
            panel.sizeDelta = new Vector2(725, choices.Count() * 46 + 12);
            for (int i = 0; i < choices.Count; i++) {
                Choice currentChoice = choices[i];
                UnityEngine.UI.Button newButton = Instantiate(dialogueOptionPrefab, panel);
                newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = currentChoice.text;
                newButton.onClick.AddListener(
                    () => {
                        choiceSelection.Invoke(currentChoice);
                        // Hide();
                    }
                    );
                newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 46 * -i - 8);
                newButton.gameObject.SetActive(true);
            }
            panel.gameObject.SetActive(true);
        }

        public void Clear()
        {
            for (int i = 0; i < panel.childCount; i++)
            {
                Destroy(panel.GetChild(i).gameObject);
            }
        }

        public void Hide()
        {
            panel.transform.gameObject.SetActive(false);
        }

        public static void Show(List<Choice> choices)
        {
            if (Instance != null) {
                Instance.Clear();
                Instance.PopulateChoices(choices);
                return;
            }
            print("DialogueUI's Instance is null. Please ensure that you have a DialogueUI prefab in the scene.");
        }

        
    }
}

using Assets.PlatformerFolder;
using Ink.UnityIntegration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Ink.Runtime;
using UnityEditor.PackageManager.Requests;

namespace Assets.Logic
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        public TextAsset inkScript;
        public Story story;

        public Func<string, string> buy = (string inp) => { return ""; };
        public Func<string, string> getPrice = (string inp) => { return ""; };
        public Func<string, string> getType = (string inp) => { return ""; };
        public Func<string> getName = () => { return ""; };

        private void Start()
        {
            Instance = this;
            story = new Story(inkScript.text);
            story.BindExternalFunction("buy", (string type) => { return buy(type); });
            story.BindExternalFunction("getPrice", (string type) => { return getPrice(type); });
            story.BindExternalFunction("getType", (string type) => { return getType(type); });
            story.BindExternalFunction("getName", () => { return getName(); });
            Dialogue req = new Dialogue()
            {
                scene = "FoodStand"
            };
            EnterScene(ref req);
            int ct = 0;
            List<Choice> choices = null;
            do
            {
                if (choices != null && choices.Count > 0)
                    Choose(ref req, choices[0]);

                string result = "";
                while (result != null)
                {
                    result = GetLine(ref req);
                    Debug.Log(result);
                }
                choices = GetChoices(ref req);
                ct++;
            } while (choices.Count > 0 && ct < 100);

        }

        private static string GrantID()
        {
            // Incredibly small chance of a duplicate flow
            return "c" + Time.realtimeSinceStartup.ToString() + "r" + UnityEngine.Random.Range(10000, 99999).ToString();
        }

        // Ensure that the request has a Flow, and that the story is using it
        private static void AcquireStory(ref Dialogue request)
        {
            // No need to worry about mutex. Unity doesn't interweave anything.

            if (request.id == null || request.id == "")
            {
                request.id = GrantID();
                Instance.buy = request.buy;
                Instance.getPrice = request.getPrice;
                Instance.getType = request.getType;
                Instance.getName = request.getName;
            }

            if (Instance.story.currentFlowName == request.id) return;

            Instance.buy = request.buy;
            Instance.getPrice = request.getPrice;
            Instance.getType = request.getType;
            Instance.getName = request.getName;

            Instance.story.SwitchFlow(request.id);
            // Instance.story.UnbindExternalFunction("buy");
            // Instance.story.BindExternalFunction("buy", (int amt) => { });

        }

        public static void BindFunc(string fnName, Action<int> func)
        {
            /* Story.ExternalFunction fn;
            if (Instance.story.TryGetExternalFunction(fnName, out fn))
            {
                if (fn. == func)
                {
                    return;
                } else
                {

                }
            }
            Instance.story.BindExternalFunction(fnName, func); */
        }

        // Enter a Knot/scene within the request's Flow
        public static void EnterScene(ref Dialogue request)
        {
            AcquireStory(ref request);
            Instance.story.ChoosePathString(request.scene);
        }

        public static void EndDialogue(ref Dialogue request)
        {
            Instance.story.RemoveFlow(request.id);
        }

        // Retrieve the next line from the NPC.
        public static string GetLine(ref Dialogue request)
        {
            AcquireStory(ref request);
            if (Instance.story.canContinue)
                return Instance.story.Continue();
            return null;
        }

        // Retrieve a list of choices the user can currently make.
        public static List<Choice> GetChoices(ref Dialogue request)
        {
            AcquireStory(ref request);
            return Instance.story.currentChoices;
        }

        // Make a choice within the current Flow, leading to another branch.
        public static void Choose(ref Dialogue request, Choice choice)
        {
            AcquireStory(ref request);
            Instance.story.ChooseChoiceIndex(choice.index);
        }

        public static Dialogue Load(string path)
        {
            string contents = System.IO.File.ReadAllText(path);
            return JsonUtility.FromJson<Dialogue>(contents);
        }

        public static bool Save(Dialogue dialogue, string path)
        {
            try
            {
                System.IO.File.WriteAllText(path, JsonUtility.ToJson(dialogue, true));
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return false;
            }
        }
    }

    public struct Dialogue
    {
        public string id;
        public string scene;
        public Dictionary<string, Action<int>> actions;
        public Func<string, string> buy;
        public Func<string, string> getPrice;
        public Func<string, string> getType;
        public Func<string> getName;
    }
}

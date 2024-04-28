using Assets.Logic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.PlatformerFolder
{
    // public UnityEvent void FloatEvent(float input);

    [Serializable]
    public class Dialogue
    {
        public DialoguePrompt initialPrompt;
        public DialoguePrompt currentPrompt;
        public DialoguePrompt[] prompts;
        public UnityEvent<string> onResponse;

        public SerializableDict<string, string> parameters; 

        public DialoguePrompt Initiate()
        {
            currentPrompt = initialPrompt;
            return currentPrompt;
        }

        public DialoguePrompt Reply(int i)
        {
            currentPrompt = prompts[i]; // currentPrompt.responseIDs[i]];

            if (currentPrompt.end) return null;

            currentPrompt.outText = currentPrompt.GetText(parameters);
            return currentPrompt;
        }
    }

    [Serializable]
    public class DialoguePrompt
    {
        public bool end = false;
        public string text;
        public string outText;

        /*public string[] acceptedResponses;
        public int[] responseIDs;*/
        public DialogueResponse[] responses;

        public string[] templated;
        // [SerializeReference] public DialoguePrompt[] responseResponses;
        public string callbackString;

        public string GetText(SerializableDict<string, string> parameters)
        {
            string output = text;
            foreach (KeyValuePair<string, string> pair in parameters) {
                output = output.Replace("{" + pair.Key + "}", pair.Value);
            }
            return output;
        }
        //public UnityEvent selected = new UnityEvent();
        //FloatEvent TakeNumber;
    }

    [Serializable]
    public struct DialogueResponse
    {
        public string text;
        public int id;
    }
}

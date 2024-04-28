using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.PlatformerFolder
{
    public interface ITalkative
    {
        public Dialogue GetDialogue();
        public string GetName();
        public string GetPrefferedTitle();
        public List<Choice> BeginDialogue();
        public List<Choice> Reply(Choice choice);
        public Vector2 GetPosition();
        public void Dismiss();
    }
}

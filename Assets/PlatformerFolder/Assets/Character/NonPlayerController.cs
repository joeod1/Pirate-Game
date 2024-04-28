using Assets.Logic;
using Assets.PlatformerFolder;
using Assets.PlatformerFolder.Assets;
using DialogueEditor;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Assets;
using Unity.VisualScripting;

public class NonPlayerController : SideController, ITalkative
{
    public GameObject target;

    public bool patroling = false;
    public bool reachedLeft = false;
    public bool reachedRight = false;
    public bool ladderBound = false;

    public Assets.Logic.Dialogue dialogue;
    public string conversationScene = "DefaultConversation";
    [SerializeField] string role;
    [SerializeField] int foodType;
    [SerializeField] bool playerKnowsName = false;

    public Port homePort;

    public LadderMap ladders;

    public int layerMask;

    public override void Start()
    {
        base.Start();

        foodType = UnityEngine.Random.Range(0, 3);

        // Conversation conv = dialogue.Deserialize();
        // SpeechNode node = conv.Root;
        // node.Text = "wowza";
        //node.Connections = new List<Connection>();
        // ((SpeechConnection)node.Connections[0]).SpeechNode
        // dialogue = DialogueManager.Load("19261dialogue.dlg");
        //DialogueManager.Save(dialogue, UnityEngine.Random.Range(0, 100000) + "dialogue.dlg");

        string[] layers = { "PlatformerCharacters" };
        layerMask = LayerMask.GetMask(layers);
    }


    float oldOverlappingX = -1f;
    float oldTargetX = -1f;

    private void MoveToPosition(Vector3 position, float distance = 0.1f)
    {
        // Handle (or not) if we're on a different level
        float floorDiff = (position.y) - (character.transform.position.y);
        int flooredFloorDiff = (int)(position.y / 3) - (int)(character.transform.position.y / 3);
        if (flooredFloorDiff != 0 || (character.onLadder > 0 && Math.Abs(floorDiff) > 0.05f))
        {
            if (character.onLadder > 0)
            {
                if (floorDiff > 0) character.ClimbLadder(300f);
                else character.ClimbLadder(-300f);
            }

            float value;
            if (flooredFloorDiff > 0) value = ladders.Get(character.transform.position.y);
            else value = ladders.Get(character.transform.position.y - 3);

            if (value != -1 && !ladderBound)
            {
                ladderBound = true;
                MoveToPosition(new Vector2(value, character.transform.position.y));
                ladderBound = false;
            }
            return;
        }

        // bool walkingIntoCharacter = Physics2D.CircleCast(character.transform.position + , 0.7f, Vector2.one, 0.7f, layerMask: layerMask, minDepth: -10).collider != null;

        if (Math.Abs(position.x - character.transform.position.x) < distance)
        {
            character.Walk(0);
            return;
        }

        if (position.x > character.transform.position.x)
        {
            character.Walk(0.5f);
        }
        else
        {
            character.Walk(-0.5f);
        }
        
        
        if (overlapping != null)
        {
            /*oldOverlappingX = overlapping.GetPosition().x;
            if (overlapping.GetPosition().x > character.transform.position.x && position.x > character.transform.position.x)
            {
                character.Walk(-0.5f);
                character.transform.localScale = new Vector3(-1, 1, 1) * 1.6f;
                return;
            }

            if (overlapping.GetPosition().x < character.transform.position.x && position.x < character.transform.position.x)
            {
                character.Walk(0.5f);
                character.transform.localScale = Vector3.one * 1.6f;
                return;
            }*/
            // character.body.AddForce(new Vector2(10f/(overlapping.GetPosition().x - character.transform.position.x), 0), ForceMode2D.Force);
        }
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;

            //if ()
            //{
            //    targetPosition -= new Vector3(1.6f, 0f);
            //}
            MoveToPosition(targetPosition, 1.6f);
        }
    }

    private void Update()
    {
        MoveToTarget();
    }

    /*public Dialogue GetDialogue()
    {
        return null;
        // return dialogue;
    }*/

    public Assets.PlatformerFolder.Dialogue GetDialogue()
    {
        dialogue = new Assets.Logic.Dialogue();
        dialogue.scene = conversationScene;
        // DialogueManager.EnterScene();
        throw new NotImplementedException();
    }

    public string GetName()
    {
        throw new NotImplementedException();
    }

    public string GetPrefferedTitle()
    {
        if (playerKnowsName)
        {
            return name + ", " + role;
        }
        return role;
    }

    public List<Choice> BeginDialogue()
    {
        character.dialogueBackground.SetActive(true);
        dialogue.scene = conversationScene;

        dialogue.buy = (string type) =>
        {
            if (type == "cannon")
            {
                if (Market.GiveCannons(ResourceType.Gold, homePort.CalculateBarter(ResourceType.Gold, ResourceType.CannonBalls) + 1))
                {
                    print("wooow! cannon bought :D");
                }
            }
            return "tried to buy";
        };
        dialogue.getType = (string type) =>
        {
            if (type == "food")
                return foodType.ToString();
            return "";
        };
        dialogue.getPrice = (string type) =>
        {
            if (type == "food")
            {
                return (homePort.CalculateBarter(ResourceType.Gold, ResourceType.Food) + 1).ToString();
            } else if (type == "orange")
            {
                return (homePort.CalculateBarter(ResourceType.Gold, ResourceType.Oranges) + 1).ToString();
            } else if (type == "cannon")
            {
                return "1000"; // Increase price depending on how many cannons the player has
                //return (homePort.CalculateBarter(ResourceType.Gold, ResourceType.CannonBalls) + 1).ToString();
            } else if (type == "cannonball")
            {
                return (homePort.CalculateBarter(ResourceType.Gold, ResourceType.CannonBalls) + 1).ToString();
            }
            return "1000";
        };
        dialogue.getName = () =>
        {
            playerKnowsName = true;
            return name;
        };

        DialogueManager.EnterScene(ref dialogue);
        character.dialogueBox.GetComponent<TMPro.TMP_Text>().text = DialogueManager.GetLine(ref dialogue);
        LayoutRebuilder.ForceRebuildLayoutImmediate(character.dialogueBox.GetComponentInParent<RectTransform>());
        return DialogueManager.GetChoices(ref dialogue);
    }

    public List<Choice> Reply(Choice choice)
    {
        DialogueManager.Choose(ref dialogue, choice);
        DialogueManager.GetLine(ref dialogue);
        string line = DialogueManager.GetLine(ref dialogue);
        if (line != null && line != "")
        {
            character.dialogueBox.GetComponent<TMPro.TMP_Text>().text = line;
            LayoutRebuilder.ForceRebuildLayoutImmediate(character.dialogueBox.GetComponentInParent<RectTransform>());
        } else
        {
            character.dialogueBackground.SetActive(false);
        }
        return DialogueManager.GetChoices(ref dialogue);
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void Dismiss()
    {
        character.dialogueBackground.SetActive(false);
    }

    private NonPlayerController overlapping = null;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<NonPlayerController>(out overlapping))
        {
            // no need to handle;
        }
    }
}


using Assets;
using Assets.Logic;
using Assets.PlatformerFolder;
using Assets.PlatformerFolder.Assets;
using Assets.Ships;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : SideController, ICanLoot
{

    public InputActionAsset actions;
    public InputActionMap inputMap;

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction climbAction;
    public InputAction shootAction;
    public InputAction swingAction;
    public InputAction interactAction;

    public Ship playerShip;
    public Ship boardedShip;
    public ResourceContainer container;

    private Skybox skybox;
    private Color initialSkyboxTint;
    private float skyboxLerp = 0;

    private ITalkative talkativeInRange;

    public override void Start()
    {
        base.Start();

        skybox = GetComponentInChildren<Skybox>();
        initialSkyboxTint = Color.clear; //(808080);//skybox.material.GetColor("_Tint");

        DialogueUI.Instance.choiceSelection.AddListener(ChoiceMade);

        InitializeControls();
    }

    public bool InitializeControls()
    {
        try
        {
            // Load and enable the topdown input map
            inputMap = actions.FindActionMap("Platformer");
            inputMap.Enable();

            // Load + enable inputs for controls
            moveAction = inputMap.FindAction("Move");
            jumpAction = inputMap.FindAction("Jump");
            climbAction = inputMap.FindAction("Climb");
            shootAction = inputMap.FindAction("Shoot");
            swingAction = inputMap.FindAction("Swing");
            interactAction = inputMap.FindAction("Interact");

            shootAction.performed += _ => character.Shoot();
            jumpAction.performed += _ => character.Jump();
            swingAction.performed += _ => character.SwingSword();
            interactAction.performed += _ => Interact();

            return true;
        }
        catch (Exception e)
        {
            print(e);
            return false;
        }
    }
    private void OnEnable()
    {
        if (inputMap != null)
        {
            inputMap.Enable();
        }
    }

    public void Interact()
    {
        if (container != null)
        {
            playerShip.cargo += container.contents;
            boardedShip.cargo -= container.contents;
            Destroy(container.gameObject);
        }

        if (talkativeInRange != null)
        {
            List<Choice> choices = talkativeInRange.BeginDialogue();
            SystemsManager.UnsetHint("Press F to speak to " + talkativeInRange.GetPrefferedTitle());
            DialogueUI.Show(choices);
        }
    }

    public void ChoiceMade(Choice choice)
    {
        if (talkativeInRange != null)
        {
            print("Alright... A choice was made");
            List<Choice> choices = talkativeInRange.Reply(choice);
            if (choices.Count > 0)
            {
                DialogueUI.Show(choices);
            } else
            {
                SystemsManager.SetHint("Press F to speak to " + talkativeInRange.GetPrefferedTitle());
                DialogueUI.Instance.Hide();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //= 1 for right and = -1 for left//
        float horizontalInput = moveAction.ReadValue<float>();  // Input.GetAxis("Horizontal");
        character.walking = horizontalInput;
        

        //track player with camera//
        Camera.main.transform.position = transform.position - new Vector3(0, 0, 10);

        //moving left and right//
        character.Walk(horizontalInput);

        if (character.onLadder > 0)
        {
            character.ClimbLadder(500 * climbAction.ReadValue<float>());

            skyboxLerp = (6 - transform.localPosition.y);
            if (skyboxLerp > 1) skyboxLerp = 1;
            else if (skyboxLerp < 0) skyboxLerp = 0;
        }

        if ((transform.localPosition.y > 6 || transform.localPosition.x < -2) && character.onLadder == 0)
        {
            try
            {
                SystemsManager.SetHint("Press F to return to your ship");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            if (interactAction.IsPressed())
            {
                character.topdownMode.SetActive(true);
                character.platformerMode.SetActive(false);
                MusicPlayer.PlayTrack(character.topdownMode.GetComponent<ActiveMapPlacer>().topdownMusic);
                inputMap.Disable();
            }
        }
        else if (transform.localPosition.y > 6 && character.onLadder != 0)
        {
            SystemsManager.UnsetHint("Press F to return to your ship");
        }
        else if (transform.localPosition.x >= -2 && transform.localPosition.y <= 6)
        {
            SystemsManager.UnsetHint("Press F to return to your ship");
        }
        else
        {
            //if (skyboxLerp < 1) skyboxLerp = skyboxLerp + 0.05f;
        }



        

        skybox.material.SetColor("_Tint", Color.Lerp(Color.grey, Color.black, skyboxLerp));


    }

    public void PromptContainer(GameObject obj)
    {
        container = obj.GetComponent<ResourceContainer>();
        SystemsManager.SetHint(container.contents.ToString() + "Press F to take");
    }

    public void LeaveContainer(GameObject obj)
    {
        if (container != null && container.gameObject == obj)
        {
            SystemsManager.UnsetHint(container.contents.ToString() + "Press F to take");
            container = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ITalkative NPC = collision.GetComponent<ITalkative>();
        if (NPC != null)
        {
            talkativeInRange = NPC;
            SystemsManager.SetHint("Press F to speak to " + NPC.GetPrefferedTitle());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ITalkative NPC = collision.GetComponent<ITalkative>();
        if (NPC != null && talkativeInRange == NPC)
        {
            DialogueUI.Instance.Hide();
            NPC.Dismiss();
            talkativeInRange = null;
            SystemsManager.UnsetHint("Press F to speak to " + NPC.GetPrefferedTitle());
        }
    }
}


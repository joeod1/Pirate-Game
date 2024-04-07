using Assets.Logic;
using Assets.PlatformerFolder.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : SideController
{

    public InputActionAsset actions;
    public InputActionMap inputMap;

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction climbAction;
    public InputAction shootAction;
    public InputAction swingAction;
    public InputAction interactAction;

    public override void Start()
    {
        base.Start();

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

    }

    // Update is called once per frame
    void Update()
    {


        /*if (animator == null) {
            print("animator not found");
        }*/

        //sets the walking animation on or off based on horizontalInput//
        /*animator.SetBool("walking", character.walking != 0 && character.isGrounded);
        if (animator.GetBool("walking"))
        {
            animator.SetBool("idle", false);
            animator.SetBool("jumping", false);
        }
        else if (!animator.GetBool("walking") && character.isGrounded)
        {
            animator.SetBool("jumping", false);
            animator.SetBool("idle", true);
        }*/



        //= 1 for right and = -1 for left//
        float horizontalInput = moveAction.ReadValue<float>();  // Input.GetAxis("Horizontal");
        character.walking = horizontalInput;
        

        //track player with camera//
        Camera.main.transform.position = transform.position - new Vector3(0, 0, 10);

        //moving left and right//
        //if (horizontalInput != 0)
            character.Walk(horizontalInput);
        
        //if (character.isGrounded && horizontalInput != 0)
        //    character.body.velocity = new Vector2(horizontalInput * character.speed, character.body.velocity.y);

        //for flipping sprite left and right depending on direction//
        /*if (horizontalInput > 0.01f)
            character.transform.localScale = new Vector3(-1, 1, 1) * 1.6f;
        else if (horizontalInput < -0.01f)
            character.transform.localScale = Vector3.one * 1.6f;*/
        
        /*

        if (character.onLadder == 0)
        {
            //up for jump//
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                character.Jump();
        }
        else
        {
            // climb up ladder //
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                character.ClimbLadder(300f);
                //character.body.AddForce(new Vector2(0, 300f * Time.deltaTime));
            }

            // climb down ladder //
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                character.ClimbLadder(-800f);
                //character.body.AddForce(new Vector2(0, -800f * Time.deltaTime));
            }
        }*/

        if (character.onLadder > 0)
        {
            character.ClimbLadder(500 * climbAction.ReadValue<float>());
        }

        //spacebar for sword swinging//
        /*if (Input.GetKey(KeyCode.Space))
            character.SwingSword();*/

        //shift or e for shoot//
        /*if (character.cooldown > 1.0f)
        {
            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.E))
            {
                character.cooldown = 0.0f;
                character.Shoot();
            }
        }
        else
        {
            character.cooldown += Time.deltaTime;
        }*/
 
        // loot containers //
        // doesn't work right now
        /*if (containerPeek != null && Input.GetKeyDown(KeyCode.F)) {
            for (int i = 0; i < containerPeek.quantities.Keys.Count; i++)
            {
                playerCargo.quantities[i] += containerPeek.quantities[i];
                containerPeek.quantities[i] = 0;
                LeftContents(containerPeek);
            }
            containerPeek.quantities.Clear();
        }*/

        if (transform.localPosition.y > 6 && character.onLadder == 0)
        {
            SystemsManager.SetHint("Press F to leave the ship");
            //character.displayInfo.text = "Press F to leave the ship";
            if (interactAction.IsPressed())//Input.GetKeyDown(KeyCode.F))
            {
                character.topdownMode.SetActive(true);
                character.platformerMode.SetActive(false);
                inputMap.Disable();
            }
        } else if (transform.localPosition.y > 6 && character.onLadder != 0)
        {
            SystemsManager.UnsetHint("Press F to leave the ship");
        }

  
    } 
 
}


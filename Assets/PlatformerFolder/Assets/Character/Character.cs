using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Character : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private Animator animator;
    public LayerMask layerMask;

    [SerializeField] private float speed;
    [SerializeField] private float jump_speed;
    [SerializeField] private float drag;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] cannonBalls; 

    public SpriteRenderer hatSR;
    public SpriteRenderer head;
    public SpriteRenderer browR;
    public SpriteRenderer browL;
    public SpriteRenderer eyeR;
    public SpriteRenderer eyeL;
    public SpriteRenderer mouth;
    public SpriteRenderer bodySR;
    public SpriteRenderer handR;
    public SpriteRenderer handL;
    public SpriteRenderer footR;
    public SpriteRenderer footL;

    public GameObject captainHat;
    public GameObject hatGO;
    private GameObject sword;
    private GameObject gun;

    //  public CharacterController controller;
    public Boolean isPlayer;
    public Boolean isFish;
    public Boolean isUndead;
    public Boolean isCaptain;

    private float walking = 0.0f;
    private float cooldown = 50.0f;


    // Start is called before the first frame update
    void Start()
    {
        captainHat = GameObject.FindGameObjectWithTag("captainHat");
        hatGO = GameObject.FindGameObjectWithTag("hat");
        sword = GameObject.FindGameObjectWithTag("sword");
        gun = GameObject.FindGameObjectWithTag("gun");
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        gun.SetActive(false);
        animator.SetFloat("walkspeed", speed);
    }

    //Update is called once per frame
    void Update()
    {
        //sets the walking animation on or off based on horizontalInput//
        animator.SetBool("walking", walking != 0 && isGrounded());
        if (animator.GetBool("walking"))
        {
            animator.SetBool("idle", false);
            animator.SetBool("jumping", false);
        }
        else if (!animator.GetBool("walking") && isGrounded())
        {
            animator.SetBool("jumping", false);
            animator.SetBool("idle", true);
        }

        //turns the gun object off and the sword object on while not shooting//
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("shoot_gun"))
        {
            gun.SetActive(false);
            sword.SetActive(true);
        }
        //controls list//
        //actual actions take place in seperate functions//
        if (isPlayer)
        {
            //= 1 for right and = -1 for left//
            float horizontalInput = Input.GetAxis("Horizontal");
            walking = horizontalInput;  
            //track player with camera//
            Camera.main.transform.position = transform.position - new Vector3(0, 0, 10);

            //moving left and right//
            if (isGrounded() && horizontalInput != 0)
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            //for flipping sprite left and right depending on direction//
            if (horizontalInput > 0.01f)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (horizontalInput < -0.01f)
                transform.localScale = Vector3.one;

            //up for jump//
            if (Input.GetKey(KeyCode.UpArrow))
                Jump();
            else if (Input.GetKey(KeyCode.W))
                Jump();

            //spacebar for sword swinging//
            if (Input.GetKey(KeyCode.Space))
                SwingSword();

            //shift or e for shoot//
            if (cooldown > 0.2f)
            {
                if (Input.GetKey(KeyCode.RightShift))
                {
                    cooldown = 0.0f;
                    Shoot();
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    cooldown = 0.0f;
                    Shoot();
                }
            }
            else 
            {
                cooldown += Time.deltaTime;
            }
        }
    } 
    public void Shoot()
    {
        sword.SetActive(false);
        gun.SetActive(true);
        animator.SetTrigger("shoot");
    }

    private void ReleaseCannonBall()
    {
        cannonBalls[0].SetActive(true);
        cannonBalls[0].transform.position = firePoint.position;
        cannonBalls[0].transform.Translate(5, 0, 0);
        cannonBalls[0].GetComponent<Projectile>().SetSpeedAndDirection(body.velocity.x, Mathf.Sign(transform.localScale.x)); 

    }



    public void SwingSword()
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("swing_sword"))
            animator.SetTrigger("swing");
    }

    public void Jump()
    {
        if (isGrounded())
        {
            animator.SetBool("walking", false);
            animator.SetBool("jumping", true);
            animator.SetBool("idle", false);
            body.velocity = new Vector2(body.velocity.x * (1 - drag), jump_speed);
        }
    }

    private bool isGrounded() {

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, layerMask) ;
        return raycastHit.collider != null;
    }
    
}

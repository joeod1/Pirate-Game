using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Camera camera;

    private Rigidbody2D body;
    private Collider2D boxCollider;
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
    public int onLadder = 0;


    public TMPro.TMP_Text displayInfo;
    public TradeResources playerCargo;
    public TradeResources containerPeek;
    public GameObject topdownMode;
    public GameObject platformerMode;
    public void InsightContents(TradeResources container)
    {
        containerPeek = container;
        DisplayContents();
    }
    public void LeftContents(TradeResources container)
    {
        if (containerPeek != container) return;
        containerPeek = null;
        displayInfo.text = "";
    }
    public void DisplayContents()
    {
        displayInfo.text = containerPeek.ToString() + "Press F to take";
    }


    // Start is called before the first frame update
    void Start()
    {
        captainHat = GameObject.FindGameObjectWithTag("captainHat");
        hatGO = GameObject.FindGameObjectWithTag("hat");
        sword = GameObject.FindGameObjectWithTag("sword");
        gun = GameObject.FindGameObjectWithTag("gun");
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        gun.SetActive(false);
        animator.SetFloat("walkspeed", speed);
    }

    public void EnterLadder()
    {
        onLadder++;
        body.gravityScale = 0f;
        body.drag = 5f;
        displayInfo.text = "";
    }

    public void ExitLadder()
    {
        onLadder--;
        body.gravityScale = 1f;
        body.drag = 0f;
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
            // loot containers //
            // doesn't work right now
            if (containerPeek != null && Input.GetKeyDown(KeyCode.F)) {
                foreach (KeyValuePair<ResourceType, int> pair in containerPeek.quantities.ToList()) {
                    playerCargo.quantities[pair.Key] += pair.Value;//containerPeek.quantities.Values[i];
                    containerPeek.quantities[pair.Key] = 0;
                }
                LeftContents(containerPeek);
                // Destroy(containerPeek);
            }

            if (transform.localPosition.y > 6)
            {
                displayInfo.text = "Press F to leave the ship";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    topdownMode.SetActive(true);
                    platformerMode.SetActive(false);
                }
            }

            //= 1 for right and = -1 for left//
            float horizontalInput = Input.GetAxis("Horizontal");
            walking = horizontalInput;  
            //track player with camera//
            camera.transform.position = transform.position - new Vector3(0, 0, 10);

            //moving left and right//
            if (isGrounded() && horizontalInput != 0)
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            //for flipping sprite left and right depending on direction//
            if (horizontalInput > 0.01f)
                transform.localScale = new Vector3(-1, 1, 1) * 1.6f;
            else if (horizontalInput < -0.01f)
                transform.localScale = Vector3.one * 1.6f;

            if (onLadder == 0)
            {
                //up for jump//
                if (Input.GetKey(KeyCode.UpArrow))
                    Jump();
                else if (Input.GetKey(KeyCode.W))
                    Jump();
            } else
            {
                // climb up ladder //
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                {
                    body.AddForce(new Vector2(0, 300f * Time.deltaTime));
                }

                // climb down ladder //
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    body.AddForce(new Vector2(0, -800f * Time.deltaTime));
                }
            }

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

        RaycastHit2D raycastHit = Physics2D.CircleCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.size.y / 2f), 0.1f, Vector2.down, 0, layerMask);//Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, layerMask) ;
        return raycastHit.collider != null;
    }
    
}

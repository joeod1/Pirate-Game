using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
   
    public Rigidbody2D body;
    private Collider2D boxCollider;
    private Animator animator;
    public LayerMask layerMask;

    [SerializeField] public float speed;
    [SerializeField] private float jump_speed;
    [SerializeField] private float drag;
    [SerializeField] private float firing_speed;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject cannonBall; 

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
    public GameObject sword;
    public GameObject gun;

    public PlayerController controller;
    public Boolean isPlayer;
    public Boolean isFish;
    public Boolean isUndead;
    public Boolean isCaptain;
    public Boolean isGrounded;


    public float walking = 0.0f;
    public float cooldown = 50.0f;
    public int onLadder = 0;
    public float direction = 0;


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
        direction = -transform.localScale.x;

    }  
            
    public void Shoot()
    {
        sword.SetActive(false);
        gun.SetActive(true);
        animator.SetTrigger("shoot");
    }

    private void ReleaseCannonBall()
    {
        GameObject newCannonBall = Instantiate(cannonBall);
        newCannonBall.transform.localScale = new Vector3(0.325f, 0.325f, 0.0f);
        newCannonBall.transform.position = firePoint.position;
        newCannonBall.GetComponent<Rigidbody2D>().velocity = new Vector2 (direction * firing_speed, newCannonBall.GetComponent<Rigidbody2D>().velocity.y);

    }

    public void SwingSword()
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("swing_sword"))
            animator.SetTrigger("swing");
    }

    public void Jump()
    {
        if (this.isGrounded)
        {
            animator.SetBool("walking", false);
            animator.SetBool("jumping", true);
            animator.SetBool("idle", false);
            body.velocity = new Vector2(body.velocity.x * (1 - drag), jump_speed);
        }
    }

    private bool checkIfGrounded() {

        RaycastHit2D raycastHit = Physics2D.CircleCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.size.y / 2f), 0.1f, Vector2.down, 0, layerMask);//Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, layerMask) ;
        return raycastHit.collider != null;
    }
    
}

using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

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

    // public PlayerController controller;
    public Boolean isPlayer;
    public Boolean isFish;
    public Boolean isUndead;
    public Boolean isCaptain;
    public Boolean isGrounded;

    public bool hostile = true;
    public bool patrolling = true;


    public float walking = 0.0f;
    public float cooldown = 50.0f;
    public bool gunPrimed = true;
    public bool gunPriming = false;
    public int onLadder = 0;
    public float direction = 0;


    public TMPro.TMP_Text displayInfo;
    public TradeResources playerCargo;
    public TradeResources containerPeek;
    public GameObject topdownMode;
    public GameObject platformerMode;

    public AudioSource audioSource;
    public AudioClip stepSound;

    public GameObject dialogueBox;
    public GameObject dialogueBackground;

    public void InsightContents(TradeResources container)
    {
        containerPeek = container;
        DisplayContents();
    }

    public void LeftContents(TradeResources container)
    {
        if (containerPeek != container) return;
        containerPeek = null;
        // displayInfo.text = "";
    }

    public void DisplayContents()
    {
        displayInfo.text = containerPeek.ToString() + "Press F to take";
    }


    // Start is called before the first frame update
    void Start()
    {
        // captainHat = GameObject.FindGameObjectWithTag("captainHat");
        // hatGO = GameObject.FindGameObjectWithTag("hat");
        // sword = GameObject.FindGameObjectWithTag("sword");
        // gun = GameObject.FindGameObjectWithTag("gun");
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
        // displayInfo.text = "";
    }

    public void ExitLadder()
    {
        onLadder--;
        if (onLadder == 0)
        {
            body.gravityScale = 1f;
            body.drag = 0f;
        }
    }

    //Update is called once per frame
    void Update()
    {
        isGrounded = checkIfGrounded();

        direction = -transform.localScale.x;

        if (isGrounded && walking == 0)
        {
            animator.SetBool("idle", true);
            animator.SetBool("jumping", false);
            animator.SetBool("walking", false);
        }
        else if (!isGrounded && onLadder == 0)
        {
            animator.SetBool("jumping", true);
            animator.SetBool("idle", false);
            animator.SetBool("walking", false);
        }

        //turns the gun object off and the sword object on while not shooting//
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("shoot_gun"))
        {
            gun.SetActive(false);
            sword.SetActive(true);
        }
    }  

    public void PlayStepSound()
    {
        if (audioSource.pitch == 0.9f)
            audioSource.pitch = 1f;
        else audioSource.pitch = 0.9f;
        audioSource.PlayOneShot(stepSound);
    }

    public void Walk(float direction)
    {
        if (isGrounded)
        {
            if (direction != 0)
            {
                animator.SetFloat("walkspeed", Mathf.Abs(direction) / 2f + 0.5f);
                animator.SetBool("walking", true);
                animator.SetBool("jumping", false);
                animator.SetBool("idle", false);
            }

            if (direction < -0.01f)
            {
                transform.localScale = Vector3.one * 1.6f;
                dialogueBox.transform.localScale = Vector3.one * 0.1f;
            }
            else if (direction > 0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1) * 1.6f;
                dialogueBox.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            }

            body.velocity = new Vector2(direction * speed, body.velocity.y);
        }
    }
            
    public void Shoot()
    {
        gunPrimed = false;
        sword.SetActive(false);
        gun.SetActive(true);
        animator.SetTrigger("shoot");
        StartCoroutine(ReloadGun(1));
    }

    public IEnumerator ReloadGun(int seconds)
    {
        if (!gunPrimed && !gunPriming)
        {
            gunPriming = true;
            sword.SetActive(false);
            gun.SetActive(true);
            yield return new WaitForSeconds(seconds);
            gunPrimed = true;
            gunPriming = false;
        }
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
        if (isGrounded && onLadder == 0)
        {
            animator.SetBool("walking", false);
            animator.SetBool("jumping", true);
            animator.SetBool("idle", false);
            body.velocity = new Vector2(body.velocity.x * (1 - drag), jump_speed);
        }
    }

    public void ClimbLadder(float speed)
    {
        if (onLadder > 0)
            body.AddForce(new Vector2(0, speed * Time.deltaTime));
    }

    private bool checkIfGrounded() {

        RaycastHit2D raycastHit = Physics2D.CircleCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.size.y / 2f), 0.1f, Vector2.down, 0, layerMask);//Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, layerMask) ;
        return raycastHit.collider != null;
    }
    
}

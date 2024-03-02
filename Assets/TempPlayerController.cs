using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerController : MonoBehaviour
{
    public Rigidbody2D rb2dD;
    public Camera camera;
    public int platformLayer;
    public int onLadder = 0;
    public string[] excludeLayers = { "Platforms" };
    // Start is called before the first frame update
    void Start()
    {
        //rb2dD = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2dD == null)
        {
            //rb2dD = GetComponent<Rigidbody2D>();
            print("No rigidbody!");
        }
        camera.transform.position = transform.position - new Vector3(0, 0, 10);
        int horizontalSpeed = 500;
        if (onLadder > 0)
        {
            horizontalSpeed *= 3;
            rb2dD.gravityScale = 0;
            rb2dD.drag = 10;
            // GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask(excludeLayers);
            if (Input.GetKey(KeyCode.W))
            {
                rb2dD.AddForce(new Vector2(0, 1000 * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb2dD.AddForce(new Vector2(0, -1000 * Time.deltaTime));
            }
        } else
        {
            // GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask();
            rb2dD.drag = 1;
            rb2dD.gravityScale = 1f;
            // rb2dD = null;
        }


        Collider2D collider = GetComponent<Collider2D>();
        Collider2D overlap = Physics2D.OverlapCircle(collider.bounds.center - new Vector3(0, 0.4f), 0.5f, LayerMask.GetMask("Platforms"));
        if (Input.GetKey(KeyCode.D))
        {
            rb2dD.AddForce(new Vector2(horizontalSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb2dD.AddForce(new Vector2(-horizontalSpeed * Time.deltaTime, 0));
        }
        if (onLadder == 0 && Input.GetKeyDown(KeyCode.W))
        {
            if (rb2dD.velocity.y == 0 || overlap != null)
            {
                rb2dD.AddForce(new Vector2(0, 250));
            }
        }

    }

}

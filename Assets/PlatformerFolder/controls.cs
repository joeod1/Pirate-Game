using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // init
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            print("working");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(2.0f, 2.0f));
        }
    }
}

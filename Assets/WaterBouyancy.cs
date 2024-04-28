using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class WaterBouyancy : MonoBehaviour
{
    public bool bouyant = false;
    public float bouyantForce = 100f;
    public float offset = 2f;
    [ReadOnly] public float thisTop = 0f;
    [ReadOnly] public float objectMid = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!bouyant) return;

        Rigidbody2D body;
        if (collision.TryGetComponent(out body))
        {
            thisTop = transform.position.y + transform.localScale.y;
            objectMid = body.transform.position.y + offset;
            if (objectMid < thisTop)
            {
                print("bouyaaant");
                body.velocity = new Vector2(body.velocity.x / 1.1f, bouyantForce);
                // body.AddForce(new Vector2(0, bouyantForce * Time.deltaTime), ForceMode2D.Impulse);
            }
        }
    }
}

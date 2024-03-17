using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceContainer : MonoBehaviour
{
    private Collider2D trigger;
    public TradeResources contents = new TradeResources();
    public Vector3 offset = new Vector3(0, 0);
    public int capacity = 10;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TempPlayerController>() != null)
        {
            // display weight
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<TempPlayerController>() != null)
        {
            // remove pop-up
        }
    }
}

using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        print(trigger);
    }

    public void Empty()
    {
        for (int i = 0; i < contents.quantities.Keys.Count; i++)
        {
            contents.quantities[contents.quantities.Keys.ElementAt(i)] = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("We can trigger");
        if (collision.GetComponent<Character>() != null)
        {
            collision.GetComponent<Character>().InsightContents(contents);
            // display weight

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>() != null)
        {
            // remove pop-up
            collision.GetComponent<Character>().LeftContents(contents);
        }
    }
}

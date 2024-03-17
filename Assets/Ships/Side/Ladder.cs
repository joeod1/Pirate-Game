using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LadderCollisions : MonoBehaviour
{
    GameObject topPlatform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TempPlayerController controller = collision.gameObject.GetComponent<TempPlayerController>();
        if (controller != null)
        {
            controller.onLadder++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TempPlayerController controller = collision.gameObject.GetComponent<TempPlayerController>();
        if (controller != null)
        {
            controller.onLadder--;
        }
    }
}

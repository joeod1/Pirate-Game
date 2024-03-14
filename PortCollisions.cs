using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortCollisions : MonoBehaviour
{
    public TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collision");
        uiText.text = "at the port";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        uiText.text = "";
    }
}

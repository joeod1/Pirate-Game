using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HappinessScript : MonoBehaviour
{
    public int happiness;
    public static int maxHappiness;
    public TMP_Text happinessText;
    // Start is called before the first frame update
    void Start()
    {
        happinessText = GetComponent<TMP_Text>();
        happiness = 50;
        maxHappiness = 100;
        happinessText.text = "Happiness: " + happiness.ToString() + "/" + maxHappiness.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
      happinessText.text = "Happiness: " + happiness.ToString() + "/" + maxHappiness.ToString();
    }
}

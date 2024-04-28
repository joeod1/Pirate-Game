using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageNotoriety : MonoBehaviour
{
    public UINotorietyScript notor;
    public static System.String manageText;
    public TMP_Text displayText;
    public static int maxLevelAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //similar if statements to UINotorietyScript, but this time assigning level and maxLevelAmount
        //6 levels in total
        if (notor.progress1 < 100){
            manageText = "UNKNOWN";
            maxLevelAmount = 100;
            displayNotoriety();
        }
        if (notor.progress1 >= 100 && notor.progress1 < 200){
            manageText = "RUMORED";
            maxLevelAmount = 200;
            displayNotoriety();
        }
        if (notor.progress1 >= 200 && notor.progress1 < 300){
            manageText = "VILLAIN";
            maxLevelAmount = 300;
            displayNotoriety();
        }
        if (notor.progress1 >= 300 && notor.progress1 < 400){
            manageText = "CAPTAIN";
            maxLevelAmount = 400;
            displayNotoriety();
        }
        if (notor.progress1 >= 400 && notor.progress1 < 499){
            manageText = "LEGEND";
            maxLevelAmount = 499;
            displayNotoriety();
        }
             if (notor.progress1 >= 499){
            manageText = "WINNER";
            maxLevelAmount = 500;
            displayNotoriety();
        }
    }
    public void displayNotoriety(){
        //Line of text that will be display in the Manage Screen. EX. Output: "Notoriety: 0/100 (UNKNOWN)"
        displayText.text = "Notoriety: " + notor.progress1.ToString() + "/" + maxLevelAmount.ToString() + " (" + manageText + ")";
    }
}

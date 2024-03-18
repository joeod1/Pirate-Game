using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UINotorietyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int progress1;
    public TMP_Text levelText;
    void Start()
    {
        // Game starts with 0 progress, level is UNKNOWN
        levelText = GetComponent<TMP_Text>();
        progress1 = 0;
        levelText.text = "UNKNOWN";
    }

    // Update is called once per frame
    void Update()
    {
        //5 main levels, UNKNOWN, RUMORED, VILLAIN, CAPTAIN, LEGEND. In addition, there is WINNER for when you beat the game
        if (progress1 < 100){
            levelText.text = "UNKNOWN";
            LowerAmountBound();
        }
        if (progress1 >= 100 && progress1 < 200){
            levelText.text = "RUMORED";
        }
        if (progress1 >= 200 && progress1 < 300){
            levelText.text = "VILLAIN";
        }
        if (progress1 >= 300 && progress1 < 400){
            levelText.text = "CAPTAIN";
        }
        if (progress1 >= 400 && progress1 < 499){
            levelText.text = "LEGEND";
        }
        HigherAmountBound();
        
    }
    //Bounds of progress
    void LowerAmountBound(){
        if (progress1 < 0){
            progress1 = 0;
        }
    }
    void HigherAmountBound(){
        if (progress1 > 500){
            progress1 = 500;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIGoldAmount : MonoBehaviour
{
    //code that sets text alongside the UI gold icon on the top right of the screen
    public static int amount;
    public static float roundedAmount;
    public static float floatAmount;
    public TMP_Text amountText;
    void Start()
    {
        amountText = GetComponent<TMP_Text>();
        amount = 100; //placeholder amount at start
        amountText.text = amount.ToString() + " " + "GOLD";
    }

    // Update is called once per frame
    void Update()
    {
        //Rounds the amount if its above a thousand
        if (amount < 1000){
            LowerAmountBound();
        amountText.text = amount.ToString() + " GOLD";
        }
        else{
            HighAmountBound();
            floatAmount = (float) amount/1000;
            roundedAmount = (float)System.Math.Round(floatAmount*10)/10;
            amountText.text = roundedAmount.ToString() + "k" + " GOLD";
        }
    }
    void LowerAmountBound(){
        if (amount < 0){
            amount = 0;
        }
    }
    public static int HighAmountBound(){
        if (amount > 1000000){
            amount = 1000000;
            return amount;
        }
        return amount;
    }
}

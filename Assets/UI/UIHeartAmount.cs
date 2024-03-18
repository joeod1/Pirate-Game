using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIHeartAmount : MonoBehaviour
{
    // code that assigns values to the text for the UI Heart icon in the top right of the screen
    public static int amount;
    public static int maxAmount;
    public TMP_Text amountText;
    void Start()
    {
        amountText = GetComponent<TMP_Text>();
        amount = 100;
        maxAmount = 100;
        amountText.text = amount.ToString() + "/" + maxAmount.ToString(); //start with 100/100 health
    }

    // Update is called once per frame
    void Update()
    {
        amountText.text = amount.ToString() + "/" + maxAmount.ToString();
    }
    void LowerAmountBound(){
        if (amount < 0){
            amount = 0;
        }
        if (maxAmount < 0){
            maxAmount = 0;
        }
    }
}

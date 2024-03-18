using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIGoldAmount : MonoBehaviour
{
    //code that sets text alongside the UI gold icon on the top right of the screen
    public static int amount;
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
        amountText.text = amount.ToString() + " GOLD";
    }
    void LowerAmountBound(){
        if (amount < 0){
            amount = 0;
        }
    }
}

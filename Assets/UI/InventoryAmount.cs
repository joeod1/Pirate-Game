using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventoryAmount : MonoBehaviour
{
    //displays placeholder for each inventory amount in the inventory screen
    public static int amount;
    public TMP_Text amountText;
        // Start is called before the first frame update
    void Start()
    {
        amountText = GetComponent<TMP_Text>();
        amount = 100;
        amountText.text = amount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LowerAmountBound(){
        if (amount < 0){
            amount = 0;
        }
    }
}

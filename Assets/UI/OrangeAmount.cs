using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OrangeAmount : MonoBehaviour
{
    //displays amount for Oranges in the inventory screen
    public static int amount = 5;
    public TMP_Text amountText;
        // Start is called before the first frame update
    void Start()
    {
        amountText = GetComponent<TMP_Text>();
        amountText.text = amount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        amountText.text = amount.ToString();
    }
    void LowerAmountBound(){
        if (amount < 0){
            amount = 0;
        }
    }
}

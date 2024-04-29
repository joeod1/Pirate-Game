using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feed1Orange : MonoBehaviour
{
    //Script that when feeding an orange, increases a crew member's happiness by 5
    public HappinessScript happy2;
    private Button feedOrangeButton;
    // Start is called before the first frame update
    void Start()
    {
        feedOrangeButton = GetComponent<Button>();
        feedOrangeButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
       if (OrangeAmount.amount >= 1 && happy2.happiness < 100){
       OrangeAmount.amount = OrangeAmount.amount - 1;
       happy2.happiness = happy2.happiness + 5;
       if (happy2.happiness > 100){
            maxBound();
       }
       }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void maxBound()
    {
        happy2.happiness = 100;
    }
    
}
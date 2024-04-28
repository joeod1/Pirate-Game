using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pay10GoldScript : MonoBehaviour
{
    public HappinessScript happy;
    //public UIGoldAmount gold;
    private Button payGoldButton;
    // Start is called before the first frame update
    void Start()
    {
        payGoldButton = GetComponent<Button>();
        payGoldButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
       if (UIGoldAmount.amount >= 10 && happy.happiness < 100){
       UIGoldAmount.amount = UIGoldAmount.amount - 10;
       happy.happiness = happy.happiness + 10;
       if (happy.happiness > 100){
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
        happy.happiness = 100;
    }
    
}

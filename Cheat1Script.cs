using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheat1Script : MonoBehaviour
{
    private Button addGoldButton;
    // Start is called before the first frame update
    void Start()
    {
        addGoldButton = GetComponent<Button>();
        addGoldButton.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TaskOnClick(){
        //Adds 100 gold when clicking the cheat button
        UIGoldAmount.amount = UIGoldAmount.amount + 100;
        UIGoldAmount.HighAmountBound();
    }
}

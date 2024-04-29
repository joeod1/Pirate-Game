<<<<<<< HEAD:Assets/UI/Cheat1Script.cs
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
        UIGoldAmount.amount = UIGoldAmount.amount + 100;
        UIGoldAmount.HighAmountBound();
    }
}
=======
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
>>>>>>> 1d2f3e3bfc47ccf7de0cbf438d6fcc3b50b5f0b6:Cheat1Script.cs

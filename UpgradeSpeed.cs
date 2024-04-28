using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    ShipController shipController;
    bool purchasable = true;
    public Button purchaseButton;
    void Start()
    {
        purchaseButton = GetComponent<Button>();
        purchaseButton.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick(){
    if (purchasable == true){
        //shipController.forwardSpeed = (float)(shipController.forwardSpeed * 1.5);
        //shipController.turningSpeed = (float)(shipController.turningSpeed * 1.5);
        }
    }
}

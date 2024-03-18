using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class InventoryButtonScript : MonoBehaviour
{
    //basic button script for the inventory button
    private Button inventoryButton;

  public MenuScript menuScreen;
  public MenuScript inventoryScreen;
    public void Start(){
        inventoryButton = GetComponent<Button>();
        inventoryButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        menuScreen.gameObject.SetActive(false);
        inventoryScreen.gameObject.SetActive(true);
    }
}

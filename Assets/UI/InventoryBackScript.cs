using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class InventoryBackScript : MonoBehaviour
{
    private Button backButton;
//button script that is the opposite of the inventory button
  public MenuScript menuScreen;
  public MenuScript inventoryScreen;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        menuScreen.gameObject.SetActive(true);
        inventoryScreen.gameObject.SetActive(false);
    }
}
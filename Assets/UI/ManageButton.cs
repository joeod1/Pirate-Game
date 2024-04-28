using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManageButton : MonoBehaviour
{
        //basic button script for the manage button
    private Button manageButton;

  public MenuScript menuScreen;
  public MenuScript manageScreen;
    public void Start(){
        manageButton = GetComponent<Button>();
        manageButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        menuScreen.gameObject.SetActive(false);
        manageScreen.gameObject.SetActive(true);
    }
}

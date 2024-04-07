using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsButtonScript : MonoBehaviour
{
    private Button settingsButton;
//basic button script for control button in settings
  public ControlsScript controlsScreen;
  public SettingsScript settingsScreen;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        controlsScreen.gameObject.SetActive(true);
        settingsScreen.gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsButtonScript : MonoBehaviour
{
    private Button settingsButton;

  public SettingsScript controlsScreen;
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
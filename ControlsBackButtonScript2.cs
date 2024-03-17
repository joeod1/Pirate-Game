using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class ControlsBackButtonScript2 : MonoBehaviour
{
    private Button settingsButton;
// back buton for the controls screen
  public ControlsScript controlsScreen;
  public SettingsScript settingsScreen;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        controlsScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(true);
    }
}
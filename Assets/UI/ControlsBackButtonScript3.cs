using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsBackButtonScript3 : MonoBehaviour
{
    private Button settingsButton;
// back buton for the controls screen
  public ControlsScript controlsScreen;
  public ControlsScript controlsScreen2;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        controlsScreen2.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(true);
    }
}

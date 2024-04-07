using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuideButton : MonoBehaviour
{
    private Button settingsButton;
//basic button script for guide button in settings
  public GuideScript guideScreen;
  public SettingsScript settingsScreen;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        guideScreen.gameObject.SetActive(true);
        settingsScreen.gameObject.SetActive(false);
    }
}

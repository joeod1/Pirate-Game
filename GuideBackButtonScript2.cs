using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuideBackButton2 : MonoBehaviour
{
    private Button settingsButton;
// back buton for the controls screen
  public GuideScript guideScreen;
  public SettingsScript settingsScreen;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        guideScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(true);
    }
}

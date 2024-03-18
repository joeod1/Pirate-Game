using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class MoreSettingsScript : MonoBehaviour
{
    //displays more settings when you click on more settings button
    private Button moreSettingsButton;
  public SettingsPg2Script settingsScreen;
    public void Start(){
        moreSettingsButton = GetComponent<Button>();
        moreSettingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        settingsScreen.gameObject.SetActive(true);
    }
}

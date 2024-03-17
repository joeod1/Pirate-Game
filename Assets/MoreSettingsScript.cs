using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoreSettingsScript : MonoBehaviour
{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreSettingsBackScript : MonoBehaviour
{
    //essentially the opposite of the MoreSettingsScript, back button on the more settings screen
      private Button backButton;
     public SettingsScript settingsScreen;
     public SettingsPg2Script settingsScreen2;
    public void Start(){
        // Start is called before the first frame update
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        settingsScreen.gameObject.SetActive(true);
        settingsScreen2.gameObject.SetActive(false);
    }
}
    // Update is called once per frame
 


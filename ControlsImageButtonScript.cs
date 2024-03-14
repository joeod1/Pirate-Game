using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsImageButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
      private Button backButton;
     public SettingsScript settingsScreen;
     public SettingsScript settingsScreen2;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        settingsScreen.gameObject.SetActive(true);
        settingsScreen2.gameObject.SetActive(false);
    }
}
    // Update is called once per frame

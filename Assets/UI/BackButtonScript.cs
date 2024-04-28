using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
      private Button backButton;
     public SettingsScript settingsScreen;
     public MenuButtonScript menu;
     public PauseButtonScript pause;
     public MenuScript titleScreen;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        //If settings menu was activated from title screen
        if (TitleSettingsButtonScript.title == true){
            titleScreen.gameObject.SetActive(true);
            TitleSettingsButtonScript.title = false;
        }
        Time.timeScale = 1;
        settingsScreen.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        pause.gameObject.SetActive(true);
        PauseButtonScript.game_paused = false;
    }
}
    // Update is called once per frame
 


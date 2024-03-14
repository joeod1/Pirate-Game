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
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        Time.timeScale = 1;
        settingsScreen.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        pause.gameObject.SetActive(true);
        PauseButtonScript.game_paused = false;
    }
}
    // Update is called once per frame
 


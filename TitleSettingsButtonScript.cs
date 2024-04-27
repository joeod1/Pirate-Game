using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleSettingsButtonScript : MonoBehaviour
{
    //when clicking on the settings button, pauses game and hides pause and menu, shows main settings screen
    private Button settingsButton;
  public SettingsScript settingsScreen;
  public PauseButtonScript pause;
  public MenuButtonScript menu;
  public MenuScript titleScreen;
  public static bool title = false;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        Time.timeScale = 0.000001f;
        PauseButtonScript.game_paused = true;
        titleScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(true);
        pause.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        title = true;
    }
}


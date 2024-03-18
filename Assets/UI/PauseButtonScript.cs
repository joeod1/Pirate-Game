using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class PauseButtonScript : MonoBehaviour
{
    //Code for pausing and unpausing the game
    public Button pauseButton; //pause icon in the bottom right of screen
  public GameObject pause;
  public MenuButtonScript menu; //menu button in the bottom right
  public SettingsButtonScript settings; //settings button in the bottom right
  public SettingsScript pauseScreen; //pause text is enabled when paused, disabled when unpaused
  public static bool game_paused = false;
    public void Start(){
        pause.SetActive(true);
        pauseButton = GetComponent<Button>();
      
        pauseButton.onClick.AddListener(TaskOnClick);
    }
    public void Update(){
        if (Input.GetKeyDown(KeyCode.P)) //pauses game when you press p, can't unpause game with 'p'
        {
            TaskOnClick();
        }
    }
    void TaskOnClick(){
        pauseScreen.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        Time.timeScale = 0.000001f; //time can't be set to 0 since inputs  won't work that way
        game_paused = true;
        pauseButton.onClick.AddListener(TaskOnClick2);
        
    }
        void TaskOnClick2(){
        pauseScreen.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        settings.gameObject.SetActive(true);
        Time.timeScale = 1;
        game_paused = false;
        pauseButton.onClick.AddListener(TaskOnClick);
    }
}

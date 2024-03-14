using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class PauseButtonScript : MonoBehaviour
{
    
    public Button pauseButton;
  public GameObject pause;
  public MenuButtonScript menu;
  public SettingsButtonScript settings;
  public SettingsScript pauseScreen;
  public static bool game_paused = false;
    public void Start(){
        pause.SetActive(true);
        pauseButton = GetComponent<Button>();
      
        pauseButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        pauseScreen.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        Time.timeScale = 0.000001f;
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

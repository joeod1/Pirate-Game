using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseButtonScript : MonoBehaviour
{
    //Code for pausing and unpausing the game
    //Got help from Unity tutorial, "How to - Basic Pause System in Unity" by Miguel Angel Yee Rebollar
    public Button pauseButton; //pause icon in the bottom right of screen
  public GameObject pause;
  public MenuButtonScript menu; //menu button in the bottom right
  public SettingsButtonScript settings; //settings button in the bottom right
  [SerializeField]
  public SettingsScript pauseScreen = null; //pause text is enabled when paused, disabled when unpaused
  public static bool game_paused; //boolean to alternate between two states
  public bool GetIsPaused() {return game_paused;}
    public void Start(){
        pause.SetActive(true);
        pauseButton = GetComponent<Button>();
        pauseButton.onClick.AddListener(TaskOnClick);

    }
    public void Update(){
        if (Input.GetKeyDown(KeyCode.P)) //pauses game when you press p and unpauses game when you press p again
        {
            TaskOnClick();       
        }
    }
    void TaskOnClick(){
        game_paused = !game_paused;
        pauseScreen.gameObject.SetActive(game_paused);
        menu.gameObject.SetActive(!game_paused);
        settings.gameObject.SetActive(!game_paused);
        Time.timeScale = game_paused ? 0.000001f : 1; //time can't be set to 0 since inputs  won't work that wa
        print("Game speed:" + Time.timeScale);
    }
}

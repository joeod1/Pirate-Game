using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class PlayButtonScript : MonoBehaviour
{
    //unused code for a playbutton
  public SettingsScript pauseScreen;
  public Button playButton;
  public GameObject play;
  public PauseButtonScript paused;
    public void Start(){
        playButton = GetComponent<Button>();
        paused.pauseButton = GetComponent<Button>();
        play.gameObject.SetActive(false);
        playButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        pauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
        PauseButtonScript.game_paused = false;
        play.SetActive(false);
        paused.pauseButton.gameObject.SetActive(true);
    }
}

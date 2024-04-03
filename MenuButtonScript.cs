using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuButtonScript : MonoBehaviour
{
    //pauses game, disables pause and settings button, show menu when you click on the menu icon or press M
    public PauseButtonScript pause;
    public SettingsButtonScript settings;
    private Button menuButton;
  public MenuScript menuScreen;
    public void Start(){
        // Start is called before the first frame update
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(TaskOnClick);
    }
    public void Update(){
                if (Input.GetKeyDown(KeyCode.M) && (PauseButtonScript.game_paused == false)) //press M when game is not paused
        {
            PauseButtonScript.game_paused = true;
            TaskOnClick();
        }
    }
    void TaskOnClick(){
         Time.timeScale = 0.000001f;
         PauseButtonScript.game_paused = true;
        menuScreen.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
    }
}

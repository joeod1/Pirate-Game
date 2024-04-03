using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuBackButtonScript : MonoBehaviour
{
      private Button backButton;
     public MenuScript menuScreen;
     public PauseButtonScript pause;
     public SettingsButtonScript settings;
     //this code unpauses the game and shows the pause and settings button when you click the back button in the menu
    public void Start(){
        // Start is called before the first frame update
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
         Time.timeScale = 1;
         PauseButtonScript.game_paused = false;
        menuScreen.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        settings.gameObject.SetActive(true);
    }
}

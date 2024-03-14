using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuBackButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
      private Button backButton;
     public MenuScript menuScreen;
     public PauseButtonScript pause;
     public SettingsButtonScript settings;
    public void Start(){
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

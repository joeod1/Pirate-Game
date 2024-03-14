using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PauseButtonScript pause;
    public SettingsButtonScript settings;
    private Button menuButton;
  public MenuScript menuScreen;
    public void Start(){
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
         Time.timeScale = 0.000001f;
         PauseButtonScript.game_paused = true;
        menuScreen.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
    }
}

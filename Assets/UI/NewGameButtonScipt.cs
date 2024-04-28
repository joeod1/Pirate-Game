using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButtonScript : MonoBehaviour
{
    public MenuScript titleScreen;
    private Button newGameButton;
    public PauseButtonScript pause;
    public SettingsButtonScript settings;
    public MenuButtonScript menu;
    // Start is called before the first frame update
    void Start()
    {
        newGameButton = GetComponent<Button>();
        newGameButton.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick(){
        titleScreen.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        settings.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
    }
}
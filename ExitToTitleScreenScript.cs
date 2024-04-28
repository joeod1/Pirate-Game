using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitToTitleScreenScript : MonoBehaviour
{
    public MenuScript titleScreen;
    public MenuScript menuScreen;
    private Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick(){
        menuScreen.gameObject.SetActive(false);
        titleScreen.gameObject.SetActive(true);
    }
}

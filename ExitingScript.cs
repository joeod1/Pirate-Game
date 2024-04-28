using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//part of code is thanks to a unity tutorial. "How to quit the game in Unity" by John French:
// https://gamedevbeginner.com/how-to-quit-the-game-in-unity/
public class ExitingScript : MonoBehaviour
{
    
    private Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(TaskOnClick);
    }
    public void TaskOnClick(){
        //quits application in either Unity Editor or in build
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}

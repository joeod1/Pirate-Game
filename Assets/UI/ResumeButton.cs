using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    //code that disables the ending screen
      private Button resumeButton;
     public EndingImage endingScreen;
    public void Start(){
        // Start is called before the first frame update
        resumeButton = GetComponent<Button>();
        resumeButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        endingScreen.gameObject.SetActive(false);
    }
}

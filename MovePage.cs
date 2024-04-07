using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePage : MonoBehaviour
{
    //basic button for viewing the controls
      private Button guideButton;
     public  GuideViewScript nextPage;
     public  GuideViewScript previousPage;

    // Start is called before the first frame update
    public void Start(){
        guideButton = GetComponent<Button>();
        guideButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        nextPage.gameObject.SetActive(true);
        previousPage.gameObject.SetActive(false);
    }
}
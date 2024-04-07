using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideViewButton : MonoBehaviour
{
    //basic button for viewing the controls
      private Button guideButton;
     public  GuideViewScript guideView;
     public  GuideScript guideScreen;

    // Start is called before the first frame update
    public void Start(){
        guideButton = GetComponent<Button>();
        guideButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        guideView.gameObject.SetActive(true);
        guideScreen.gameObject.SetActive(false);
    }
}
    // Update is called once per frame

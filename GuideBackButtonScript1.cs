using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBackButtonScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    //Goes back from viewing specific guide screen to main guide screen
      private Button backButton;
     public  GuideViewScript guideView;
     public  GuideScript guideScreen;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        guideView.gameObject.SetActive(false);
        guideScreen.gameObject.SetActive(true);
    }
}
    // Update is called once per frame

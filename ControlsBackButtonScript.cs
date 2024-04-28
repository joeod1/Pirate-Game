using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsBackButtonScript : MonoBehaviour
{
    //A back button used in the controls section
    // Start is called before the first frame update
      private Button backButton;
     public  ControlsViewScript controlsView;
     public  ControlsScript controlsScreen;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        controlsView.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(true);
    }
}
    // Update is called once per frame
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsImageButtonScript : MonoBehaviour
{
    //basic button for viewing the controls
      private Button backButton;
     public  ControlsViewScript controlsView;
     public  ControlsScript controlsScreen;

    // Start is called before the first frame update
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        controlsView.gameObject.SetActive(true);
        controlsScreen.gameObject.SetActive(false);
    }
}
    // Update is called once per frame

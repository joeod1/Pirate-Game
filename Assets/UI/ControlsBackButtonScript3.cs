<<<<<<< HEAD:Assets/UI/ControlsBackButtonScript3.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsBackButtonScript3 : MonoBehaviour
{
    private Button settingsButton;
// back buton for the controls screen
  public ControlsScript controlsScreen;
  public ControlsScript controlsScreen2;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        controlsScreen2.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(true);
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsBackButtonScript3 : MonoBehaviour
{
    private Button settingsButton;
// back buton for the controls screen's back button
  public ControlsScript controlsScreen;
  public ControlsScript controlsScreen2;
    public void Start(){
        settingsButton = GetComponent<Button>();
        settingsButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        controlsScreen2.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(true);
    }
}
>>>>>>> 1d2f3e3bfc47ccf7de0cbf438d6fcc3b50b5f0b6:ControlsBackButtonScript3.cs

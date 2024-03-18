using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class ManageBackScript : MonoBehaviour
{
    private Button backButton;
//button script that is the opposite of the manage button
  public MenuScript menuScreen;
  public MenuScript manageScreen;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        menuScreen.gameObject.SetActive(true);
        manageScreen.gameObject.SetActive(false);
    }
}

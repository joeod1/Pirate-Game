using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class CreditsButtonScript : MonoBehaviour
{
    private Button backButton;

  public MenuScript menuScreen;
  public MenuScript creditsScreen;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        menuScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(true);
    }
}
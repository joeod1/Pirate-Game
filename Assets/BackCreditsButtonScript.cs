using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackCreditsButtonScript : MonoBehaviour
{
    private Button backButton;

  public MenuScript menuScreen;
  public MenuScript creditsScreen;
    public void Start(){
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        menuScreen.gameObject.SetActive(true);
        creditsScreen.gameObject.SetActive(false);
    }
}
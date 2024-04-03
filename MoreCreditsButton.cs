using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoreCreditsBUtton : MonoBehaviour
{
    //basic button for viewing credits
    private Button moreButton;

  public MenuScript creditsScreen;
  public MenuScript nextCreditsScreen;
    public void Start(){
        moreButton = GetComponent<Button>();
        moreButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick(){
        creditsScreen.gameObject.SetActive(false);
        nextCreditsScreen.gameObject.SetActive(true);
    }
}

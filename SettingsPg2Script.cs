using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsPg2Script : MonoBehaviour
{
    //initially hides the page for extra settings
    public static GameObject settings;
    public void Start(){
        HideSetting();
    }
    public void HideSetting(){
        settings.SetActive(false);
    }
    
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class SettingsScript : MonoBehaviour
{
    //initially hides main settings screen
    public static GameObject settings; //static is used so buttons that refer to it work better
    public void Start(){
        HideSetting();
    }
    public void HideSetting(){
        settings.SetActive(false);
    }
}

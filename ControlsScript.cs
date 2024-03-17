using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class ControlsScript : MonoBehaviour
{
    //disables main control screen at the start
    public static GameObject controls;
    public void Start(){
        HideControls();
    }
    public void HideControls(){
        controls.SetActive(false);
    }
}

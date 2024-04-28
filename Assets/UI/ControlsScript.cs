using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsScript : MonoBehaviour
{
    //disables main control screen at the start
    public static GameObject controls;
    public void Start(){
        controls = gameObject;
    }
    public void HideControls(){
        controls.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlsViewScript : MonoBehaviour
{
    //disables screen at the start where you view specific controls for a specific function
    public static GameObject controls;
    public void Start(){
        controls = gameObject;
    }
    public void HideControls(){
        controls.SetActive(false);
    }
}

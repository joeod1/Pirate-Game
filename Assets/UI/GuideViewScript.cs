using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuideViewScript : MonoBehaviour
{
    //disables screen at the start where you view specific controls for a specific function
    public static GameObject guide;
    public void Start(){
        HideGuide();
    }
    public void HideGuide(){
        guide.SetActive(false);
    }
}


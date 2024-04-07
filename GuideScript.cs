using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuideScript : MonoBehaviour
{
    //disables main guide screen at the start
    public static GameObject guide;
    public void Start(){
        HideGuide();
    }
    public void HideGuide(){
        guide.SetActive(false);
    }
}

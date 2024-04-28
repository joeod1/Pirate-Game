using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour
{
    //hides all menu screens in the menu canvas at the start
    public static GameObject menu;
    public void Start(){
        menu = gameObject;
        
    }
    public void HideMenu(){
        menu.SetActive(false);
    }
    
}

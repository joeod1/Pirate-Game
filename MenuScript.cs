using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour
{
    public GameObject menu;
    public void Start(){
        menu.SetActive(false);
    }
    
}

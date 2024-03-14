using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
public class SettingsScript : MonoBehaviour
{
    public GameObject settings;
    public void Start(){
        settings.SetActive(false);
    }
    
}

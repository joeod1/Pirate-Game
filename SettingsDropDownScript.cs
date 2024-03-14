using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class SettingsDropDownScript : MonoBehaviour
{
    public TMP_Dropdown m_Dropdown;
    
    public Resolution[] resolutionData;
    // Start is called before the first frame update
    void Start()
    {
          m_Dropdown = GetComponent<TMP_Dropdown>();
          Debug.Log("Starting");
        PopulateDropdown(); 
    }
void PopulateDropdown() {
  Debug.Log("In PopulateDropdown()");
  m_Dropdown.ClearOptions();
  Debug.Log("Clear Options");
  resolutionData = Screen.resolutions;
  List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
  foreach (Resolution res in resolutionData) {
    // add to dropdown
    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
    option.text = res.height + "x" + res.width;
    options.Add(option);
  }
  
   m_Dropdown.AddOptions(options);
   Debug.Log("Options Added");
}
public void OnPointerClick(BaseEventData data) {
int index = m_Dropdown.value;
Screen.SetResolution(resolutionData[index].width, resolutionData[index].height, true);
}
}

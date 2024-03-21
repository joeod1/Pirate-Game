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
    void Start()
    {
        // Start is called before the first frame update
        m_Dropdown = GetComponent<TMP_Dropdown>();
        Debug.Log("Starting");   //debug logs are here cause I had trouble making the dropdown menu work
        m_Dropdown.onValueChanged.AddListener(selectValue => OnDropdownSelect());
        PopulateDropdown();
    }
    void PopulateDropdown()
    {
        Debug.Log("In PopulateDropdown()");
        m_Dropdown.ClearOptions(); //clears dropdown options when you start game
        Debug.Log("Clear Options");
        resolutionData = Screen.resolutions;
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (Resolution res in resolutionData)
        {
            // add to dropdown
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = res.height + "x" + res.width;
            options.Add(option); //add resolution options to dropdown
        }

        m_Dropdown.AddOptions(options); //adds all options to dropdownmenu
        Debug.Log("Options Added");
    }
    public void OnDropdownSelect()
    {
        int index = m_Dropdown.value;
        Screen.SetResolution(resolutionData[index].width, resolutionData[index].height, true); //sets each resolution to dropdownmenu based on its index
    }
}
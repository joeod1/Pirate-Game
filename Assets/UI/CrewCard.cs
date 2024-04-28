using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrewCard : MonoBehaviour
{
    public static GameObject crewCard;
    public static new String name;
    public  TMP_Text nameText;
    // Start is called before the first frame update
    void Start()
    {
        crewCard = gameObject;
        nameText.text = name;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

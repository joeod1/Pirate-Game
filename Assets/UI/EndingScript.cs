using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
public class EndingScript : MonoBehaviour
{
    public  EndingImage ending; //Refers to Ending Screen

    public UINotorietyScript notor; //refers to Notoriety Text
    public static bool game_win = false; //boolean to be used in VictoryCondition()

    public void Start(){
        HideEnding();
    }
    public void Update()
    {
       
      notor.progress1 = VictoryCondition(notor.progress1);  //checks for win condition and updates notor.progress1 to 500
    }
    public void HideEnding(){
        ending.gameObject.SetActive(false); //sets ending screen to inactive
    }
    public int VictoryCondition(int progress){ //requires progress of notoriety and for the ending screen to not already appear
         if (progress >= 499 && game_win == false){
            notor.levelText.text = "WINNER";
            ending.gameObject.SetActive(true);
            progress = 500;
            game_win = true;
            return progress;
         }
         return progress;
    }
}

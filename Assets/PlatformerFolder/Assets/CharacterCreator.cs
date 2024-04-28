using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterCreator : MonoBehaviour { 
    
    public Sprite[] hats;
    public Sprite[] accessories;
    public Sprite[] weapons;
    public Sprite[] heads;
    public Sprite[] brows;
    public Sprite[] eyes;
    public Sprite[] eye_affectations;
    public Sprite[] mouths;
    public Sprite[] bodies;
    public Sprite[] right_hands;
    public Sprite[] left_hands;
    public Sprite[] feet;

    public void fillCharacter(Character character) {
        
        character.captainHat.SetActive(character.isCaptain);
        character.hatGO.SetActive(!character.isCaptain);
       
        if (character.isFish)
        {
            CreateFishCharacter(character);
        }
        //else if (character.isUndead)
       // {
        //    CreateUndeadCharacter(character);
       // }
        else 
        {
            CreateHumanCharacter(character);    
        }
    }

    public void CreateFishCharacter(Character character) {

        /* fish person */

        //assign all the sprites

        character.captainHat.SetActive(false);
        character.hatGO.SetActive(true);

            character.hatSR.sprite = hats[4];
            character.hatSR.transform.localPosition += new Vector3(0.0f, 0.27f, 0.0f);
     
        character.head.sprite = heads[2];
        character.browL.sprite = brows[1];
        character.browR.sprite = brows[1];
        character.bodySR.sprite = bodies[UnityEngine.Random.Range(2,5)];    
        character.mouth.sprite = mouths[1];
        character.handL.sprite = left_hands[1];
        character.handR.sprite = right_hands[1];
        character.footR.sprite = feet[1];
        character.footL.sprite = feet[1];

        //local position changes to match up on the same model for animations
       // character.handL.transform.localPosition += new Vector3(-0.12f, 0.03f, 0.0f);
        character.handR.transform.localPosition += new Vector3(0.0f, 0.04f, 0.0f);
        character.footL.transform.localPosition += new Vector3(0.0f, -0.06f, 0.0f);
        character.footR.transform.localPosition += new Vector3(0.0f, -0.06f, 0.0f);
    }

    /* private void CreateUndeadCharacter(Character character) {
        // zombie 
        
        //assign all the sprites
        if (!character.isCaptain) { character.hatSR.sprite = hats[3]; }       //randomHat() 
        character.head.sprite = heads[1];
        character.browL.sprite = brows[0];
        character.browR.sprite = brows[0];
        character.bodySR.sprite = bodies[UnityEngine.Random.Range(3, 5)];   
        character.mouth.sprite = mouths[1];
        character.handL.sprite = left_hands[1];
        character.handR.sprite = right_hands[1];
        character.footR.sprite = feet[1];
        character.footL.sprite = feet[1];
        character.eyeL.sprite = eyes[1];
        character.eyeR.sprite = eyes[1];

        //local position changes to match up on the same model for animations
        character.handL.transform.localPosition += new Vector3(-0.12f, 0.03f, 0.0f);
        character.handR.transform.localPosition += new Vector3(0.0f, 0.04f, 0.0f);
        character.footL.transform.localPosition += new Vector3(0.0f, -0.06f, 0.0f);
        character.footR.transform.localPosition += new Vector3(0.0f, -0.06f, 0.0f);
 
    } */

    public void CreateHumanCharacter(Character character)
    {

        character.captainHat.SetActive(false);
        character.hatGO.SetActive(true);

        if (!character.isCaptain) { character.hatSR.sprite = hats[3]; }       //randomHat() 
      //  if (character.isPlayer) { character.head.sprite = heads[3]; }
        character.head.sprite = heads[UnityEngine.Random.Range(0, 2)];         
        // character.browL.sprite = brows[2];
        //character.browR.sprite = brows[2];
        character.bodySR.sprite = bodies[UnityEngine.Random.Range(0, 4)];   
        //character.mouth.sprite = mouths[1];
       // character.handL.sprite = left_hands[3];
        //character.handR.sprite = right_hands[3];
        //character.footR.sprite = feet[3];
        //character.footL.sprite = feet[3];
        //character.eyeL.sprite = eyes[2];
        //character.eyeR.sprite = eyes[2];

    }

}



//each hat has a different positioning issue and the fin should only be assigned to fish people
//and 2 of the hats look awkward on the fish people cuz its the color of their skin
/*
private void setHatSprite(Boolean isFish = false) {
        int min = 0;    
        int max = 4;    //5 is the fin
        if (isFish) {
            min = 3;    //0-2 are the color of the fish people and look weird
            max = 5; 
        }

        int hatNumber = UnityEngine.Random.Range(min, max);
        SpriteRenderer hat = GameObject.FindGameObjectWithTag("hat").GetComponent<SpriteRenderer>();
        hat.sprite = hats[0];   //hatNumber];

        if (hatNumber == 0)
        {
            hat.transform.localPosition += new Vector3(0.015f, 0.1f, 0.0f);
        }
        else if (hatNumber == 1)
        {

        }
        else if (hatNumber == 2)
        {
            hat.transform.localPosition = new Vector3(0.0f, -0.1f, 0.0f);
        }
        else if (hatNumber == 3) {
            hat.transform.localPosition += new Vector3(0.0f, -0.2f, 0.0f);
        }
        else if (hatNumber == 4)
        {
            hat.transform.localPosition += new Vector3(-0.05f, 0.1f, 0.0f);
        }
        else {
            print("unkownerror... hatNumer= ");
            print(hatNumber);
        }
    }

}
*/
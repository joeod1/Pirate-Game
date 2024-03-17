using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SceneController : MonoBehaviour{

    private CharacterCreator creator;
    [SerializeField] public GameObject enemy1;
    [SerializeField] public GameObject enemy2;

    // Start is called before the first frame update
    void Start()
    {
        creator = GetComponent<CharacterCreator>();
        enemy1 = Instantiate(enemy1);
        enemy2 = Instantiate(enemy2);
        enemy1.transform.position = new Vector3(-2.0f, 0.0f, 0.0f);
        enemy2.transform.position = new Vector3(-2.0f, 0.0f, 0.0f);

        enemy1.GetComponent<Character>().isFish = true;
        enemy1.GetComponent<Character>().isCaptain = false;
        enemy1.GetComponent<Character>().isUndead = false;
        enemy1.GetComponent<Character>().isPlayer = false;

        enemy2.GetComponent<Character>().isPlayer = false;
        enemy2.GetComponent<Character>().isFish = true;
        enemy2.GetComponent<Character>().isCaptain = false;
        enemy2.GetComponent<Character>().isUndead = false;

        creator.fillCharacter(enemy1.GetComponent<Character>());
        creator.fillCharacter(enemy2.GetComponent<Character>());

    }

    // Update is called once per frame
    void Update()
    {
     
        


    }
}

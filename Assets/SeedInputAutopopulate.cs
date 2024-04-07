using Assets.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedInputAutopopulate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(coPopulateField());
    }

    IEnumerator coPopulateField()
    {
        print("Waiting for that pesky seed to not be zero...");
        while (GameManager.Config.seed == -1) yield return null;
        print("Populating field now!");
        GetComponent<TMPro.TMP_InputField>().text = GameManager.Config.seed.ToString();
    }
}

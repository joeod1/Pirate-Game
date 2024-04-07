using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class LoadingBar : MonoBehaviour
{
    public UnityEvent<string, int, int> loadingFunctions;
    public TMPro.TMP_Text label;
    public RectTransform progress;
    public RectTransform subProgress;

    // Start is called before the first frame update
    void Start()
    {
        loadingFunctions.AddListener(UpdateBar);
    }

    public void UpdateBar(string text, int value, int max)
    {
        label.text = text;
        progress.localScale = new Vector3(value / (float)(max), 1);
    }

    public void UpdateSubBar(int value, int max)
    {
        subProgress.localScale = new Vector3(value / (float)(max), 0.3f);
    }
}

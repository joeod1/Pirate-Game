using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public float scale = 1f;
    public void VolumeUpdate(float vol)
    {
        GetComponent<AudioSource>().volume = vol * scale;
    }
}

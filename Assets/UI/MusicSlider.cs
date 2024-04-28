using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

/*Credit to ChrisKurhan for this code. Title: "Audio Slider in Unity Done RIGHT | Unity Tutorial 
Link Here: https://forum.unity.com/threads/audio-slider-in-unity-done-right-unity-tutorial.1247068/
*/
public class MusicSlider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    public List <AudioSource> AudioSources;
    [SerializeField]
    private TextMeshProUGUI ValueText;
    [SerializeField]
    public AudioMixMode MixMode;
    public static MusicSlider Instance;

    public void OnChangeSlider(float Value){
        ValueText.SetText($"{Value.ToString("N4")}");
        foreach (AudioSource source in AudioSources){
            if (source == null){
                AudioSources.Remove(source);
                continue;
            }
            switch(MixMode){
                case AudioMixMode.LinearAudioSourceVolume:
                source.volume = Value;
                break;
                case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + Value * 80));
                break;
                case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(Value)*20);
                break;
                
            }
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Awake(){
        MusicSlider.Instance = this;
    }
      public enum AudioMixMode{
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}

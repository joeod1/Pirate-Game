using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System.Linq;

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

    public string MixerGroupVolumeParameter = "Volume";

    public void OnChangeSlider(float Value){
        ValueText.SetText($"{Value.ToString("N4")}");
            switch(MixMode){
                case AudioMixMode.LinearAudioSourceVolume:
                    foreach (AudioSource source in AudioSources.ToList())
                    {
                        if (source == null)
                        {
                            AudioSources.Remove(source);
                            continue;
                        }
                        source.volume = Value;
                    }
                    break;
                case AudioMixMode.LinearMixerVolume:
                    Mixer.SetFloat(MixerGroupVolumeParameter, (-80 + Value * 80));
                    break;
                case AudioMixMode.LogrithmicMixerVolume:
                    Mixer.SetFloat(MixerGroupVolumeParameter, Mathf.Log10(Value)*20);
                    break;
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace Assets.Logic
{
    public class MusicPlayer : MonoBehaviour
    {
        static MusicPlayer Instance;
        public List<AudioSource> audioSources = new List<AudioSource>();
        public int currentAudioSource = 0;

        private void Awake()
        {
            Instance = this;
        }

        public IEnumerator FadeOutAudioSource(AudioSource source)
        {
            float volume = 1.0f;
            while (volume > 0f)
            {
                volume -= Time.deltaTime;
                source.volume = volume;
                yield return null;
            }
            source.Stop();
        }

        public static void PlayTrack(AudioClip track)
        {
            Instance.StartCoroutine(
                Instance.FadeOutAudioSource(Instance.audioSources[Instance.currentAudioSource])
                );

            Instance.currentAudioSource = (Instance.currentAudioSource + 1) % Instance.audioSources.Count;
            Instance.audioSources[Instance.currentAudioSource].clip = track;
            Instance.audioSources[Instance.currentAudioSource].volume = 1f;
            Instance.audioSources[Instance.currentAudioSource].Play();
        }
    }
}

using Code.Scripts.Internal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Code.Scripts.Game
{
    public class MenuAudioManager: SingletonMonoBehaviour<MenuAudioManager>
    {
        [Header("References")]
        public AudioSource MusicSource;
        public AudioSource FXSource;
        public AudioMixer Master;

        public Scrollbar MusicVolume;
        public Scrollbar FXVolume;

        private void Start()
        {
            UpdateBackgroundVolume();
            UpdateSFXVolume();
        }

        public void PlayFX(AudioClip fx)
        {
            FXSource.Stop();
            FXSource.clip = fx;
            FXSource.Play();
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            MusicSource.Stop();
            MusicSource.clip = clip;
            MusicSource.loop = loop;
            MusicSource.Play();
        }

        public void Stop(AudioSource source)
        {
            source.Stop();
        }

        public void UpdateBackgroundVolume()
        {
            float decibels = -80 * (1 - MusicVolume.value);
            Master.SetFloat("BackgroundVolume", decibels);
        }
        
        public void UpdateSFXVolume()
        {
            float decibels = -80 * (1 - FXVolume.value) + 10;
            Master.SetFloat("SFXVolume", decibels);
        }
    }
}

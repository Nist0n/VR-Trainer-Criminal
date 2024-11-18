using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class VolumeSettings : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider soundSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SoundVolume"))
            {
                LoadVolume();
            }
            else
            {
                SetMusicVolume();
                SetSoundVolume();
            }
        }

        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("Music", MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void SetSoundVolume()
        {
            float volume = soundSlider.value;
            audioMixer.SetFloat("Sound", MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SoundVolume", volume);
        }

        private void LoadVolume()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.value = musicVolume;

            float soundVolume = PlayerPrefs.GetFloat("SoundVolume");
            soundSlider.value = soundVolume;
        }
    }
}

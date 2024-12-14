namespace SpaceGame
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Audio;

    public class SettingsMenu : Menu
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("MusicVolume") == false)
            {
                PlayerPrefs.SetFloat("MusicVolume", 0.5f);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("SFXVolume") == false)
            {
                PlayerPrefs.SetFloat("SFXVolume", 0.5f);
                PlayerPrefs.Save();
            }
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        public override IEnumerator OpenAsync()
        {
            yield return base.OpenAsync();
            Refresh();
        }

        public void OnMusicChanged(float value)
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
            _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        }

        public void OnSFXChanged(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();
            _mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
        }

        private void Refresh()
        {
            var musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            var sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            _musicSlider.normalizedValue = musicVolume;
            _sfxSlider.normalizedValue = sfxVolume;
            _mixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20f);
            _mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20f);
        }
    }
}
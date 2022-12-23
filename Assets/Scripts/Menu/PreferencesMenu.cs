using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Utils.Data;

namespace Menu
{
    public class PreferencesMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        [SerializeField] private Slider masterAudioSlider;
        [SerializeField] private Slider sfxAudioSlider;
        [SerializeField] private Slider musicAudioSlider;
        [SerializeField] private Slider uiAudioSlider;

        // [SerializeField] private Toggle fullScreenToggle;
        // [SerializeField] private TMP_Dropdown resolution;

        private void MasterSliderChanged(float value)
        {
            audioMixer.SetFloat(GameConstants.PlayerPrefs.MasterVolume, value);
            PlayerPrefs.SetFloat(GameConstants.PlayerPrefs.MasterVolume, value);
            PlayerPrefs.Save();
        }

        private void Start()
        {
            masterAudioSlider.onValueChanged.AddListener(MasterSliderChanged);
            sfxAudioSlider.onValueChanged.AddListener(SfxSliderChanged);
            musicAudioSlider.onValueChanged.AddListener(MusicSliderChanged);
            uiAudioSlider.onValueChanged.AddListener(UISliderChanged);

            // fullScreenToggle.onValueChanged.AddListener(FullScreenToggle);
            // resolution.onValueChanged.AddListener(ResolutionChanged);

            masterAudioSlider.value = PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.MasterVolume, 1);
            sfxAudioSlider.value = PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.SfxVolume, 1);
            musicAudioSlider.value = PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.MusicVolume, 1);
            uiAudioSlider.value = PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.UIVolume, 1);

            // fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen") == 1;
            // resolution.value = PlayerPrefs.GetInt("ResolutionIndex");
        }

        private void SfxSliderChanged(float value)
        {
            audioMixer.SetFloat(GameConstants.PlayerPrefs.SfxVolume, value);
            PlayerPrefs.SetFloat(GameConstants.PlayerPrefs.SfxVolume, value);
            PlayerPrefs.Save();
        }

        private void MusicSliderChanged(float value)
        {
            audioMixer.SetFloat(GameConstants.PlayerPrefs.MusicVolume, value);
            PlayerPrefs.SetFloat(GameConstants.PlayerPrefs.MusicVolume, value);
            PlayerPrefs.Save();
        }

        private void UISliderChanged(float value)
        {
            audioMixer.SetFloat(GameConstants.PlayerPrefs.UIVolume, value);
            PlayerPrefs.SetFloat(GameConstants.PlayerPrefs.UIVolume, value);
            PlayerPrefs.Save();
        }

        private static void FullScreenToggle(bool value)
        {
            Screen.fullScreen = value;
            PlayerPrefs.SetInt("FullScreen", value ? 1 : 0);
            PlayerPrefs.Save();
        }

        // private void ResolutionChanged(int index)
        // {
            // var option = resolution.options[index].text;
            // var w = int.Parse(option.Split('x')[0]);
            // var h = int.Parse(option.Split('x')[1]);
            // Screen.SetResolution(w, h, fullScreenToggle.isOn);
            // PlayerPrefs.SetInt("ResolutionIndex", index);
            // PlayerPrefs.SetInt("ResolutionWidth", w);
            // PlayerPrefs.SetInt("ResolutionHeight", h);
            // PlayerPrefs.Save();
        // }
    }
}
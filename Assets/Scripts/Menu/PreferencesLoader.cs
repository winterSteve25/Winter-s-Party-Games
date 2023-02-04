using UnityEngine;
using UnityEngine.Audio;
using Utils.Data;

namespace Menu
{
    public class PreferencesLoader : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
    
        private void Start()
        {
            audioMixer.SetFloat(GameConstants.PlayerPrefs.MasterVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.MasterVolume, 1));
            audioMixer.SetFloat(GameConstants.PlayerPrefs.SfxVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.SfxVolume, 1));
            audioMixer.SetFloat(GameConstants.PlayerPrefs.MusicVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.MusicVolume, 1));
            audioMixer.SetFloat(GameConstants.PlayerPrefs.UIVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.UIVolume, 1));
            Screen.fullScreen = PlayerPrefs.GetInt(GameConstants.PlayerPrefs.Fullscreen, 1) == 1;
            Destroy(gameObject);
        }
    }
}
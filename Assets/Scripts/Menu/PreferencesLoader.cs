using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Menu
{
    public class PreferencesLoader : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
    
        private void Start()
        {
            audioMixer.SetFloat(GameConstants.PlayerPrefs.MasterVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.MasterVolume, 1));
            audioMixer.SetFloat(GameConstants.PlayerPrefs.SFXVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.SFXVolume, 1));
            audioMixer.SetFloat(GameConstants.PlayerPrefs.MusicVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.MusicVolume, 1));
            audioMixer.SetFloat(GameConstants.PlayerPrefs.UIVolume, PlayerPrefs.GetFloat(GameConstants.PlayerPrefs.UIVolume, 1));
            Destroy(gameObject);
        }
    }
}
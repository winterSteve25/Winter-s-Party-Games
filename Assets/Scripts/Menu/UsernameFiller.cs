#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using Steamworks;
using Steamworks.NET;
using TMPro;
using UnityEngine;
using Utils.Data;
using Utils.Dictionary;

namespace Menu
{
    public class UsernameFiller : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
#if !DISABLESTEAMWORKS
            if (SteamManager.Initialized)
            {
                inputField.text = SteamFriends.GetPersonaName().Replace("\r", "");
            } else
#endif
            {
                if (PlayerPrefs.HasKey(GameConstants.PlayerPrefs.Username))
                {
                    inputField.text = PlayerPrefs.GetString(GameConstants.PlayerPrefs.Username);
                    return;
                }
                
                var word = (WordDictionary.GetRandomAdjective() + " " + WordDictionary.GetRandomNoun()).Replace("\r", "");
                inputField.text = word;
            }
        }
    }
}
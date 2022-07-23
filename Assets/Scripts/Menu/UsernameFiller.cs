using Steamworks;
using TMPro;
using UnityEngine;
using Utils;

namespace Menu
{
    public class UsernameFiller : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            if (SteamManager.Initialized)
            {
                inputField.text = SteamFriends.GetPersonaName().Replace("\r", "");
            } else
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
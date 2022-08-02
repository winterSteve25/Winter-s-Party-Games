using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;

namespace Menu
{
    public class UsernameSaveButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        
        public void ConfirmUsernameButtonClicked()
        {
            if (string.IsNullOrEmpty(inputField.text)) return;
            var localPlayerNickName = inputField.text.Replace("\r", "");
            inputField.text = string.Empty;
            PhotonNetwork.LocalPlayer.NickName = localPlayerNickName;
            PlayerPrefs.SetString(GameConstants.PlayerPrefs.Username, localPlayerNickName);
            PlayerPrefs.Save();
        }
    }
}
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
            var localPlayerNickName = inputField.text.Replace("\r", "");
            PhotonNetwork.LocalPlayer.NickName = localPlayerNickName;
            PlayerPrefs.SetString(GameConstants.PlayerPrefs.Username, localPlayerNickName);
            PlayerPrefs.Save();
        }
    }
}
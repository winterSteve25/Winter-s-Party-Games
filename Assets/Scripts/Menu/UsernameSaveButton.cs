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
            PlayerPrefs.SetString(GameConstants.PlayerPrefs.Username, inputField.text);
            PlayerPrefs.Save();
        }
    }
}
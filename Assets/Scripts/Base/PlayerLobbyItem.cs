using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    public class PlayerLobbyItem : MonoBehaviour
    {
        public PlayerLobbyItemData data;

        private void Start()
        {
            if (LobbyData.Instance == null)
            {
                Debug.LogError("Player Lobby Item exist when LobbyData does not");
                Destroy(gameObject);
            }
        }

        public void UpdateAppearance()
        {
            if (data == null) return;
            GetComponentInChildren<TextMeshProUGUI>().text = data.nickname;
            GetComponent<Image>().sprite = LobbyData.Instance.gameMode.playerAvatars[data.avatarIndex];
        }
    }
}
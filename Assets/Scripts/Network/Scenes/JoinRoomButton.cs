using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;

namespace Network.Scenes
{
    public class JoinRoomButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField joinInput;
        
        public void JoinRoomButtonClicked()
        {
            if (string.IsNullOrEmpty(joinInput.text)) return;
            GlobalData.Set(GameConstants.GlobalData.IsHost, false);
            GlobalData.Set(GameConstants.GlobalData.RoomIDToJoin, joinInput.text.Replace("\r", "").Replace(" ", ""));
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.JoiningRoom, false);
        }
    }
}
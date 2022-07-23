using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;

namespace Network.Scenes
{
    public class JoinRoomButton : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField joinInput;
        
        public void JoinRoomButtonClicked()
        {
            GlobalData.Set(GameConstants.GlobalData.IsHost, false);
            PhotonNetwork.JoinRoom(joinInput.text);
        }

        public override void OnJoinedRoom()
        {
            SceneTransition.TransitionToScene(RoomData.Read<GameConstants.SceneIndices>(GameConstants.CustomRoomProperties.Scene));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            // TODO show error
        }
    }
}
using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;

namespace Network.Scenes
{
    public class RoomJoiner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        private void Start()
        {
            text.text = "Joining...";
            PhotonNetwork.JoinRoom(GlobalData.Read<string>(GameConstants.GlobalData.RoomIDToJoin));
        }
        
        public override void OnJoinedRoom()
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(RoomData.Read<GameConstants.SceneIndices>(GameConstants.CustomRoomProperties.Scene));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            text.text = "Failed to join room.\nReason: " + message;
        }
    }
}
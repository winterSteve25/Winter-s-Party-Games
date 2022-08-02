#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;
using System.Collections;
using Network;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;

namespace Steamworks.NET
{
    public class SteamLobbyJoiner : MonoBehaviourPunCallbacks
    {
#if !DISABLESTEAMWORKS
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject backButton;

        private Callback<LobbyEnter_t> _lobbyEntered;
        private Callback<LobbyDataUpdate_t> _lobbyDataUpdated;

        private bool _gotData;
        private bool _error;
        private bool _success;

        private void Start()
        {
            backButton.SetActive(false);
            _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            _lobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdated);
            SteamMatchmaking.JoinLobby(GlobalData.Read<CSteamID>(GameConstants.GlobalData.SteamLobbyIDToJoin));
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            Debug.Log("Joined Steam lobby");
            // if we are host we are already automatically joining the room. So only join room if we are not host
            if (GlobalData.ExistAnd<bool>(GameConstants.GlobalData.IsHost, isHost => isHost)) return;
            GlobalData.Set(GameConstants.GlobalData.IsHost, false);
            var lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            StartCoroutine(JoinRoom(lobbyID));
        }

        private void OnLobbyDataUpdated(LobbyDataUpdate_t callback)
        {
            if (callback.m_bSuccess == Convert.ToByte(false))
            {
                _error = true;
            }
            else
            {
                _success = true;
            }

            _gotData = true;
        }

        private IEnumerator JoinRoom(CSteamID lobbyID)
        {
            text.text = "Fetching Data...";
            SteamMatchmaking.RequestLobbyData(lobbyID);
            yield return new WaitUntil(() => _gotData);

            var tries = 0;

            while (_error && tries < 3)
            {
                _gotData = false;
                SteamMatchmaking.RequestLobbyData(lobbyID);
                yield return new WaitUntil(() => _gotData);
                tries++;
            }

            if (_success)
            {
                text.text = "Joining...";
                PhotonNetwork.JoinRoom(SteamMatchmaking.GetLobbyData(lobbyID,
                    GameConstants.SteamworksLobbyData.PhotonRoom));
            }
            else
            {
                text.text = "Failed to fetch necessary data...";
                yield return new WaitForSeconds(2f);
                SceneTransition.TransitionToScene(GameConstants.SceneIndices.MainMenu);
            }
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(
                RoomData.Read<GameConstants.SceneIndices>(GameConstants.CustomRoomProperties.Scene));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            backButton.SetActive(true);
            text.text = "Failed to join room.\nReason: " + message;
        }
#endif
    }
}
using Photon.Pun;
using Steamworks;
using UnityEngine;
using Utils;

namespace Network.Steam
{
    // This will be attached to the host's game so that other people can join the lobby, but it does not handle joining rooms as those will be on the client
    public class SteamLobbyCreator : MonoBehaviour
    {
        private Callback<LobbyCreated_t> _lobbyCreated;
 
        private void Start()
        {
            if (!SteamManager.Initialized) return;
            _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, PhotonNetwork.CurrentRoom.MaxPlayers);
        }

        private static void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK) return;
            Debug.Log("Steam Lobby successfully created");
            if (!SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), GameConstants.SteamworksLobbyData.PhotonRoom, PhotonNetwork.CurrentRoom.Name))
            {
                Debug.LogError("Something went wrong when setting photon room name in steam lobby");
            }
        }
    }
}
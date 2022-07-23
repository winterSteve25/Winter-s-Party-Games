using Photon.Pun;
using Steamworks;
using UnityEngine;
using Utils;

namespace Network.Steam
{
    public class SteamLobbyConnector : MonoBehaviourPunCallbacks
    {
        private Callback<GameLobbyJoinRequested_t> _requestJoinLobby;

        private static SteamLobbyConnector _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
        
            Destroy(gameObject);
        }
        
        private void Start()
        {
            if (!SteamManager.Initialized) return;
            _requestJoinLobby = Callback<GameLobbyJoinRequested_t>.Create(OnRequestJoinLobby);
        }

        private static void OnRequestJoinLobby(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Requested to join Steam lobby");
            GlobalData.Set(GameConstants.GlobalData.SteamLobbyID, callback.m_steamIDLobby);
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.JoiningRoomViaSteam);
        }
    }
}
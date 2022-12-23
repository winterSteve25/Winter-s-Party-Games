#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using UnityEngine;
using Utils;
using Utils.Data;

namespace Steamworks.NET
{
    public class SteamLobbyConnector : MonoBehaviour
    {
        #if !DISABLESTEAMWORKS
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
            GlobalData.Set(GameConstants.GlobalDataKeys.SteamLobbyIDToJoin, callback.m_steamIDLobby);
            SceneManager.TransitionToScene(GameConstants.SceneData.joinViaSteam);
        }
        #endif
    }
}
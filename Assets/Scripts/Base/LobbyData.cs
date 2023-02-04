using System;
using System.Collections.Generic;
using Games.Base;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Data;

namespace Base
{
    public class LobbyData : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// Always available in a game lobby (through out the game too. Destroyed when exiting game
        /// </summary>
        public static LobbyData Instance;

        public bool IsHost { get; private set; }
        public PartyGame PartyGame => gameMode;
        public Transform PlayerWorldSpaceDisplay => playerWorldSpaceDisplay;
        
        [SerializeField] private Transform playerWorldSpaceDisplay;
        [SerializeField] private PartyGame gameMode;
        [NonSerialized] public List<PlayerLobbyItemData> Players;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                Instance = null;
            }
            
            Instance = this;
            IsHost = GlobalData.ExistAnd(GameConstants.GlobalDataKeys.IsHost, isHost => isHost);
            DontDestroyOnLoad(gameObject);
        }

        public override void OnEnable()
        {
            SceneManager.activeSceneChanged += SceneChanged;
        }

        public override void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneChanged;
        }

        private void SceneChanged(Scene oldScene, Scene newScene)
        {
            foreach (Transform child in playerWorldSpaceDisplay)
            {
                Destroy(child.gameObject);
            }
        }
        
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Players.RemoveAll(data => data.actorID == otherPlayer.ActorNumber);
        }
    }
}
using System;
using System.Collections.Generic;
using Games.Base;
using UnityEngine;
using Utils;

namespace Base
{
    public class LobbyData : MonoBehaviour
    {
        /// <summary>
        /// Always available in a game lobby (through out the game too. Destroyed when exiting game
        /// </summary>
        public static LobbyData Instance;
        
        public PartyGame gameMode;

        [NonSerialized] public List<PlayerLobbyItemData> Players;
        [NonSerialized] public bool IsHost;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                IsHost = GlobalData.ExistAnd<bool>(GameConstants.GlobalData.IsHost, isHost => isHost);
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }
    }
}
using System;
using System.Collections.Generic;
using Games.Base;
using UnityEngine;
using Utils.Data;

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
        public bool IsHost { get; private set; }

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
    }
}
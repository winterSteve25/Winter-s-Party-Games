using System;
using Games.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Base
{
    [Serializable]
    public class PlayerLobbyDisplay
    {
        public PlayerLobbyItem PlayerLobbyItemPrefab => useDefaultLobbyItem ? Resources.Load<PlayerLobbyItem>("Prefabs/PlayerLobbyItem") : playerLobbyItem;
        public PlayerLobbySprite PlayerLobbySprite => playerLobbySprite;
        
        [SerializeField] private bool useDefaultLobbyItem = true;

        [SerializeField, DisableIf("@useDefaultLobbyItem")]
        private PlayerLobbyItem playerLobbyItem;

        [SerializeField]
        private PlayerLobbySprite playerLobbySprite;
    }
}
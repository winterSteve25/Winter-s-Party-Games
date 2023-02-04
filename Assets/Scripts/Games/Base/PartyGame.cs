using Base;
using ExitGames.Client.Photon;
using KevinCastejon.MoreAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Data;

namespace Games.Base
{
    [CreateAssetMenu(fileName = "New Party Game", menuName = "Winter's Party Games/New Party Game")]
    public class PartyGame : ScriptableObject
    {
        [Scene] public string roomScene;
        [Scene] public string gameScene;
        [MinValue(2)] public byte maximumPlayers;
        [MinValue(1)] public byte minimumPlayers;
        public PlayerLobbyDisplay[] playerAvatars;

        [Required] public Sprite gameModePreview;
        [Required] public string gameModeName;
        [Required, TextArea] public string description;
        
        public Hashtable CustomRoomProperties => new() { { GameConstants.CustomRoomProperties.Scene, roomScene } };
    }
}
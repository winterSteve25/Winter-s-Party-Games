using ExitGames.Client.Photon;
using UnityEngine;
using Utils;

namespace Games.Base
{
    [CreateAssetMenu(fileName = "New Party Game", menuName = "Winter's Party Games/New Party Game")]
    public class PartyGame : ScriptableObject
    {
        public GameConstants.SceneIndices roomScene;
        public GameConstants.SceneIndices gameScene;
        public byte maximumPlayers = 0;
        public byte minimumPlayers = 0;
        public Sprite[] playerAvatars;
        
        public Hashtable CustomRoomProperties => new() { { GameConstants.CustomRoomProperties.Scene, roomScene } };
    }
}
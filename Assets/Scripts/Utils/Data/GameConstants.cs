using Games.Base;
using Steamworks;
using UnityEngine;

namespace Utils.Data
{
    public static class GameConstants
    {
        private static SceneData _dataInstance;
        public static SceneData SceneData => _dataInstance ??= Resources.Load<SceneData>("Data/Scene Data Instance");
        
        public static class PlayerPrefs
        {
            public const string Username = "Username";
            public const string MasterVolume = "MasterVolume";
            public const string MusicVolume = "MusicVolume";
            public const string UIVolume = "UIVolume";
            public const string SfxVolume = "SFXVolume";
        }

        public static class CustomRoomProperties
        {
            public const string Scene = "s";
        }

        public static class CustomPlayerProperties
        {
            public const string AvatarIndex = "a";
            public const string IsHost = "h";
            public const string Loaded = "l";
        }

        public static class SteamworksLobbyData
        {
            public const string PhotonRoom = "prn";
        }

        public static class GlobalDataKeys
        {
            public static readonly DataSignature<bool> IsHost = new("IsHost");
            public static readonly DataSignature<CSteamID> SteamLobbyIDToJoin = new("SteamLobbyIDToJoin");
            public static readonly DataSignature<string> RoomIDToJoin = new("RoomIDToJoin");
        }

        public static class Flags
        {
            public const int BotPlayerActorID = -99;
        }

        public enum Sounds
        {
            SelectableHover,
            PlayerJoinLobby,
            PlayerLeaveLobby,
            SceneTransition,
            SceneTransitionFinish,
            PopUp,
            Tune1,
            Score
        }

        public enum SoundCategory
        {
            Music,
            UI,
            Sfx,
            Master
        }
    }
}
namespace Utils
{
    public static class GameConstants
    {
        public static class PlayerPrefs
        {
            public const string Username = "Username";
            public const string MasterVolume = "MasterVolume";
            public const string MusicVolume = "MusicVolume";
            public const string UIVolume = "UIVolume";
            public const string SFXVolume = "SFXVolume";
        }

        public enum SceneIndices
        {
            ConnectingToServer = 0,
            MainMenu = 1,
            CreateOrJoinRoom = 2,
            CreateRoom = 3,
            JoinRoom = 4,
            JoiningRoomViaSteam = 5,
            JoiningRoom = 6,
            ScrambledEggsOfDoomLobby = 7,
            ScrambledEggsOfDoomStage1 = 8,
            ScrambledEggsOfDoomStage2 = 9,
            ScrambledEggsOfDoomStage3 = 10,
            ScrambledEggsOfDoomStage4 = 11,
            ScrambledEggsOfDoomStage5 = 12,
            ScrambledEggsOfDoomVoting = 13,
            ScrambledEggsOfDoomResult = 14,
            Preferences = 15,
        }

        public static class CustomRoomProperties
        {
            public const string Scene = "s";
        }

        public static class CustomPlayerProperties
        {
            public const string AvatarIndex = "avi";
            public const string IsHost = "h";
        }

        public static class SteamworksLobbyData
        {
            public const string PhotonRoom = "prn";
        }

        public static class GlobalData
        {
            public const string IsHost = "IsHost";
            public const string SteamLobbyIDToJoin = "SteamLobbyIDToJoin";
            public const string RoomIDToJoin = "RoomIDToJoin";
            
            public const string ScrambledEggsGameData = "ScrambledEggsGameData";
            public const string ScrambledEggsVoteResult = "ScrambledEggsVoteResult";
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
            PopUp
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
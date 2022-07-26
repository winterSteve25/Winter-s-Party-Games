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
            Preferences = 2,
            CreateOrJoinRoom = 3,
            CreateRoom = 4,
            JoinRoom = 5,
            JoiningRoomViaSteam = 6,
            JoiningRoom = 7,
            ScrambledEggsOfDoomLobby = 8,
            ScrambledEggsOfDoomR1S1 = 9,
            ScrambledEggsOfDoomR1S2 = 10,
            ScrambledEggsOfDoomR1S3 = 11,
            ScrambledEggsOfDoomR1S1B = 12,
            ScrambledEggsOfDoomR2S4 = 13,
            ScrambledEggsOfDoomR2S5 = 14,
            ScrambledEggsOfDoomR2S6 = 15,
            ScrambledEggsOfDoomR2S7 = 16,
            ScrambledEggsOfDoomR2S2B = 17,
            ScrambledEggsOfDoomR3S8 = 18,
            ScrambledEggsOfDoomR3S9 = 19,
            ScrambledEggsOfDoomR3S10 = 20,
            ScrambledEggsOfDoomR3S11 = 21,
            ScrambledEggsOfDoomR3S12 = 22,
            ScrambledEggsOfDoomR3S3B = 23,
            ScrambledEggsOfDoomVoting = 24,
            ScrambledEggsOfDoomResult = 25,
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
            
            public const string LatestGameData = "GameData";
            public const string LatestVoteResult = "VoteResult";
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
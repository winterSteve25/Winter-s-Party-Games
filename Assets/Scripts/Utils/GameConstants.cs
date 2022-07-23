namespace Utils
{
    public static class GameConstants
    {
        public static class PlayerPrefs
        {
            public const string Username = "Username";
        }

        public enum SceneIndices
        {
            ConnectingToServer = 0,
            MainMenu = 1,
            CreateOrJoinRoom = 2,
            CreateRoom = 3,
            JoinRoom = 4,
            JoiningRoomViaSteam = 5,
            ScrambledEggsOfDoomLobby = 6,
            ScrambledEggsOfDoomStage1 = 7
        }

        public static class CustomRoomProperties
        {
            public const string Scene = "s";
        }

        public static class CustomPlayerProperties
        {
            public const string AvatarIndex = "avi";
        }

        public static class SteamworksLobbyData
        {
            public const string PhotonRoom = "prn";
        }

        public static class GlobalData
        {
            public const string IsHost = "IsHost";
            public const string SteamLobbyID = "SteamLobbyID";

            public const string ScrambledEggsGameData = "ScrambledEggsGameData";
            
        }
    }
}
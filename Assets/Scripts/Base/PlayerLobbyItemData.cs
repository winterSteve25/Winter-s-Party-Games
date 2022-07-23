using System;
using Photon.Realtime;

namespace Base
{
    [Serializable]
    public struct PlayerLobbyItemData
    {
        public string nickname;
        public int avatarIndex;
        public int actorID;
        
        public PlayerLobbyItemData(Player player)
        {
            nickname = player.NickName;
            // TODO
            // avatarIndex = PlayerData.Read<int>(player, GameConstants.CustomPlayerProperties.AvatarIndex);
            avatarIndex = 0;
            actorID = player.ActorNumber;
        }
    }
}
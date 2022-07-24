using System;
using Network;
using Photon.Realtime;
using Utils;

namespace Base
{
    [Serializable]
    public class PlayerLobbyItemData
    {
        public string nickname;
        public int avatarIndex;
        public int actorID;
        public int slotTaken;
        
        public PlayerLobbyItemData(Player player, int avatarIndex)
        {
            nickname = player.NickName;
            this.avatarIndex = avatarIndex;
            PlayerData.Set(player, GameConstants.CustomPlayerProperties.AvatarIndex, avatarIndex);
            actorID = player.ActorNumber;
        }
        
        public PlayerLobbyItemData(Player player)
        {
            nickname = player.NickName;
            avatarIndex = PlayerData.Read<int>(player, GameConstants.CustomPlayerProperties.AvatarIndex);
            actorID = player.ActorNumber;
        }
    }
}
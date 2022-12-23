using Base;
using Photon.Pun;
using UnityEngine;
using Utils.Data;

namespace Games.GameModes.BaubleBlitz
{
    public class BaubleBlitzPlayerSpawner : MonoBehaviour
    {
        private void Start()
        {
            var avatar = PlayerData.Read<int>(PhotonNetwork.LocalPlayer, GameConstants.CustomPlayerProperties.AvatarIndex);
            var avatarObj = LobbyData.Instance.gameMode.playerAvatars[avatar];
            var playerChar = PhotonNetwork.Instantiate("Games/Bauble Blitz/Players/" + avatarObj.Prefab.name, Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.TagObject = playerChar;
        }
    }
}
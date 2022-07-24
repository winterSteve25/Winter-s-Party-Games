using System.Collections.Generic;
using System.Linq;
using Games.Base;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Base
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected GameObject[] hostObjects;
        [SerializeField] protected GameObject[] playerObjects;

        [SerializeField] protected TextMeshProUGUI roomCodeText;
        [SerializeField] protected Button startGameButton;

        [SerializeField] protected PlayerLobbyItem[] availablePlayerSlots;
        protected List<PlayerLobbyItem> AllPlayerSlots;
        [SerializeField] protected PartyGame gameMode;

        protected List<int> AvailableAvatarIndices;
        protected bool IsHost;
        protected int SelfAvatarChoice;

        protected virtual void Awake()
        {
            if (GlobalData.ExistAnd<bool>(GameConstants.GlobalData.IsHost, isHost => isHost))
            {
                foreach (var obj in playerObjects)
                {
                    obj.SetActive(false);
                }

                IsHost = true;
            }
            else
            {
                foreach (var obj in hostObjects)
                {
                    obj.SetActive(false);
                }

                IsHost = false;
            }
    
            // adds all available avatars
            AvailableAvatarIndices = new List<int>();

            for (var i = 0; i < gameMode.playerAvatars.Length; i++)
            {
                AvailableAvatarIndices.Add(i);
            }

            // remove the already taken avatars
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values.Where(player =>
                         player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber))
            {
                AvailableAvatarIndices.Remove(PlayerData.Read<int>(player, GameConstants.CustomPlayerProperties.AvatarIndex));
            }

            // find a non taken avatar and remove it from the list of available avatars
            var avatarIndex = AvailableAvatarIndices[Random.Range(0, AvailableAvatarIndices.Count)];
            AvailableAvatarIndices.Remove(avatarIndex);
            SelfAvatarChoice = avatarIndex;

            var p = PhotonNetwork.LocalPlayer;
            PlayerData.Set(p, GameConstants.CustomPlayerProperties.AvatarIndex, avatarIndex);
            PlayerData.Set(p, GameConstants.CustomPlayerProperties.IsHost, IsHost);
        }

        protected virtual void Start()
        {
            // all slots is a copy of available player slots, used to restore slots when players leave
            AllPlayerSlots = new List<PlayerLobbyItem>(availablePlayerSlots);
            
            // disable all player slots
            foreach (var slot in availablePlayerSlots)
            {
                slot.gameObject.SetActive(false);
            }

            // assign a new avatar to the player lobby item if it is this current player
            // and adds a player to the lobby visuals
            LobbyData.Instance.Players = PhotonNetwork.CurrentRoom.Players.Select(pair =>
            {
                PlayerLobbyItemData data;
                var player = pair.Value;
                
                if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    data = new PlayerLobbyItemData(player, SelfAvatarChoice);
                }
                else
                {
                    data = new PlayerLobbyItemData(player);
                }

                AddPlayerToNextAvailableSlot(data);
                return data;
            }).ToList();

            roomCodeText.text += PhotonNetwork.CurrentRoom.Name;
            CheckStartButtonAvailability();
            
            photonView.RPC(nameof(JoinGame), RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber, SelfAvatarChoice);
        }

        public virtual void StartGame()
        {
            photonView.RPC(nameof(StartGameRPC), RpcTarget.All);
        }

        protected virtual void CheckStartButtonAvailability()
        {
            if (IsHost)
            {
                startGameButton.interactable = LobbyData.Instance.gameMode.minimumPlayers == 0 ||
                                               LobbyData.Instance.Players.Count >=
                                               LobbyData.Instance.gameMode.minimumPlayers;
            }
        }

        [PunRPC]
        private void JoinGame(int actorId, int chosenAvatar)
        {
            var data = new PlayerLobbyItemData(PhotonNetwork.CurrentRoom.GetPlayer(actorId), chosenAvatar);
            AddPlayerToNextAvailableSlot(data);
            LobbyData.Instance.Players.Add(data);
            CheckStartButtonAvailability();
        }

        [PunRPC]
        private void StartGameRPC()
        {
            SceneTransition.TransitionToScene(LobbyData.Instance.gameMode.gameScene);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogError(otherPlayer.IsMasterClient);
            RestorePlayerSlot(otherPlayer);
            LobbyData.Instance.Players.RemoveAll(p => p.actorID == otherPlayer.ActorNumber);
            
            // if the player left is the host
            if (PlayerData.Read<bool>(otherPlayer, GameConstants.CustomPlayerProperties.IsHost))
            {
                // if this player is the first player on the list after the host
                // make this player the host
                if (LobbyData.Instance.Players.First().actorID == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    IsHost = true;
                    GlobalData.Set(GameConstants.GlobalData.IsHost, true);
                    PlayerData.Set(PhotonNetwork.LocalPlayer, GameConstants.CustomPlayerProperties.IsHost, true);
                }
            }
            CheckStartButtonAvailability();
        }

        private void RestorePlayerSlot(Player player)
        {
            var data = LobbyData.Instance.Players.Find(p => p.actorID == player.ActorNumber);
            var slot = AllPlayerSlots[data.slotTaken];
            slot.gameObject.SetActive(false);
            availablePlayerSlots[data.slotTaken] = slot;
        }
        
        private void AddPlayerToNextAvailableSlot(PlayerLobbyItemData data)
        {
            var slot = availablePlayerSlots.First(element => element is not null);
            data.slotTaken = AllPlayerSlots.IndexOf(slot);
            availablePlayerSlots[data.slotTaken] = null;
            slot.gameObject.SetActive(true);
            slot.data = data;
            slot.UpdateAppearance();
        }
    }
}
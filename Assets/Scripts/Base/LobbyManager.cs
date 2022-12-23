using System.Collections.Generic;
using System.Linq;
using Games.Base;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Audio;
using Utils.Data;

namespace Base
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        // [InfoBox("All objects in this list will only be initialized when the player is the host")]
        protected GameObject[] hostObjects;
        
        [SerializeField] 
        // [InfoBox("All objects in this list will only be initialized when the player is not host")]
        protected GameObject[] playerObjects;

        [SerializeField] protected TextMeshProUGUI roomCodeText;
        [SerializeField] protected Button startGameButton;

        protected List<PlayerLobbyItem> AllPlayerSlots;
        [SerializeField] protected PlayerLobbyItem[] availablePlayerSlots;
        [SerializeField] protected PartyGame gameMode;

        [SerializeField] protected GameObject hostCrown;

        protected List<int> AvailableAvatarIndices;
        protected bool IsHost;
        protected int SelfAvatarChoice;
        protected PlayerLobbyItem SelfSlot;

        protected virtual void Awake()
        {
            if (GlobalData.ExistAnd(GameConstants.GlobalDataKeys.IsHost, isHost => isHost))
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
                AvailableAvatarIndices.Remove(PlayerData.Read<int>(player,
                    GameConstants.CustomPlayerProperties.AvatarIndex));
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
            CheckObjectsAvailability();
            if (IsHost)
            {
                hostCrown.GetComponent<RectTransform>().SetPositionAndRotation(SelfSlot.GetCrownLocation(), SelfSlot.GetCrownRotation());
            }
            else
            {
                RefreshCrown();
            }

            photonView.RPC(nameof(JoinGame), RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer.ActorNumber, SelfAvatarChoice);
        }

        public virtual void StartGame()
        {
            photonView.RPC(nameof(StartGameRPC), RpcTarget.AllBufferedViaServer);
        }

        protected virtual void CheckObjectsAvailability()
        {
            if (IsHost)
            {
                foreach (var obj in playerObjects)
                {
                    obj.SetActive(false);
                }

                foreach (var obj in hostObjects)
                {
                    obj.SetActive(true);
                }

                startGameButton.interactable = LobbyData.Instance.gameMode.minimumPlayers == 0 ||
                                               LobbyData.Instance.Players.Count >=
                                               LobbyData.Instance.gameMode.minimumPlayers;
            }
            else
            {
                foreach (var obj in hostObjects)
                {
                    obj.SetActive(false);
                }

                foreach (var obj in playerObjects)
                {
                    obj.SetActive(true);
                }
            }
        }

        [PunRPC]
        private void JoinGame(int actorId, int chosenAvatar)
        {
            var data = new PlayerLobbyItemData(PhotonNetwork.CurrentRoom.GetPlayer(actorId), chosenAvatar);
            // if this player already exist in the list of players this is a false notification, we skip it.
            if (LobbyData.Instance.Players.Any(datas => datas.actorID == actorId)) return;
            SoundManager.Play(GameConstants.Sounds.PlayerJoinLobby);
            LobbyData.Instance.Players.Add(data);
            AddPlayerToNextAvailableSlot(data);
            CheckObjectsAvailability();
        }

        [PunRPC]
        private void StartGameRPC()
        {
            SceneManager.TransitionToScene(LobbyData.Instance.gameMode.gameScene);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            SoundManager.Play(GameConstants.Sounds.PlayerLeaveLobby);
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
                    GlobalData.Set(GameConstants.GlobalDataKeys.IsHost, true);
                    PlayerData.Set(PhotonNetwork.LocalPlayer, GameConstants.CustomPlayerProperties.IsHost, true);

                    hostCrown.GetComponent<RectTransform>().position = SelfSlot.GetCrownLocation();
                    hostCrown.GetComponent<RectTransform>().rotation = SelfSlot.GetCrownRotation();
                }
            }

            CheckObjectsAvailability();

            if (!IsHost)
            {
                RefreshCrown();
            }
        }

        private void RestorePlayerSlot(Player player)
        {
            var data = LobbyData.Instance.Players.Find(p => p.actorID == player.ActorNumber);
            var slot = AllPlayerSlots[data.slotTaken];
            slot.ShrinkIcon(() =>
            {
                slot.gameObject.SetActive(false);
                availablePlayerSlots[data.slotTaken] = slot;
            });
        }

        private void AddPlayerToNextAvailableSlot(PlayerLobbyItemData data)
        {
            var slot = availablePlayerSlots.First(element => element is not null);
            data.slotTaken = AllPlayerSlots.IndexOf(slot);
            availablePlayerSlots[data.slotTaken] = null;
            slot.gameObject.SetActive(true);
            slot.ZoomIcon();
            slot.data = data;
            slot.UpdateAppearance();

            if (data.actorID == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                SelfSlot = slot;
            }
        }

        private void RefreshCrown()
        {
            foreach (var slot in AllPlayerSlots)
            {
                var dataActorID = slot.data.actorID;
                if (dataActorID == PhotonNetwork.LocalPlayer.ActorNumber) continue;
                var p = PhotonNetwork.CurrentRoom.GetPlayer(dataActorID);
                if (p is null) continue;
                var isHost = PlayerData.Read<bool>(p, GameConstants.CustomPlayerProperties.IsHost);

                if (!isHost) continue;
                
                hostCrown.GetComponent<RectTransform>().SetPositionAndRotation(slot.GetCrownLocation(), slot.GetCrownRotation());
                break;
            }
        }
    }
}
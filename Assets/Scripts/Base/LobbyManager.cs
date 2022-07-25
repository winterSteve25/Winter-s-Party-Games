using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Games.Base;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Audio;

namespace Base
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected GameObject[] hostObjects;
        [SerializeField] protected GameObject[] playerObjects;

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
            PhotonNetwork.IsMessageQueueRunning = true;

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
                hostCrown.GetComponent<RectTransform>().position = SelfSlot.GetCrownLocation();
                hostCrown.GetComponent<RectTransform>().rotation = SelfSlot.GetCrownRotation();
            }
            else
            {
                RefreshCrown();
            }

            photonView.RPC(nameof(JoinGame), RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber, SelfAvatarChoice);
        }

        public virtual void StartGame()
        {
            photonView.RPC(nameof(StartGameRPC), RpcTarget.All);
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
            SoundManager.Play(GameConstants.Sounds.PlayerJoinLobby);
            var data = new PlayerLobbyItemData(PhotonNetwork.CurrentRoom.GetPlayer(actorId), chosenAvatar);
            AddPlayerToNextAvailableSlot(data);
            LobbyData.Instance.Players.Add(data);
            CheckObjectsAvailability();
        }

        [PunRPC]
        private void StartGameRPC()
        {
            SceneTransition.TransitionToScene(LobbyData.Instance.gameMode.gameScene);
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
                    GlobalData.Set(GameConstants.GlobalData.IsHost, true);
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
            slot.GetComponentInChildren<Image>().GetComponent<RectTransform>().DOScale(Vector3.zero, 0.1f).OnComplete(
                () =>
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
            var rectTransform = slot.GetComponentInChildren<Image>().GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.zero;
            slot.gameObject.SetActive(true);
            rectTransform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutCubic);
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

                if (isHost)
                {
                    hostCrown.GetComponent<RectTransform>().position = slot.GetCrownLocation();
                    hostCrown.GetComponent<RectTransform>().rotation = slot.GetCrownRotation();
                    break;
                }
            }
        }
    }
}
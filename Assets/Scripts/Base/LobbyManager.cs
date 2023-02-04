using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
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
        [SerializeField, FoldoutGroup("Side Specific Objects")]
        [Tooltip("All objects in this list will only be initialized when the player is the host")]
        private GameObject[] hostObjects;
        
        [SerializeField, FoldoutGroup("Side Specific Objects")] 
        [Tooltip("All objects in this list will only be initialized when the player is not host")]
        private GameObject[] playerObjects;

        [SerializeField, Required] private TextMeshProUGUI roomCodeText;
        [SerializeField, Required] private Button startGameButton;

        private List<PlayerLobbyItem> _allPlayerSlots;
        [SerializeField] private PlayerLobbyItem[] availablePlayerSlots;
        [SerializeField] private GameObject hostCrown;

        [SerializeField, Required] private TextMeshProUGUI startingCountDown;
        [SerializeField, Required] private GameObject cancelButton;

        private List<int> _availableAvatarIndices;
        private bool _isHost;
        private int _selfAvatarChoice;
        private PlayerLobbyItem _selfSlot;

        private void Awake()
        {
            if (GlobalData.ExistAnd(GameConstants.GlobalDataKeys.IsHost, isHost => isHost))
            {
                foreach (var obj in playerObjects)
                {
                    obj.SetActive(false);
                }

                _isHost = true;
            }
            else
            {
                foreach (var obj in hostObjects)
                {
                    obj.SetActive(false);
                }

                _isHost = false;
            }

            // adds all available avatars
            _availableAvatarIndices = new List<int>();

            for (var i = 0; i < FindObjectOfType<LobbyData>().PartyGame.playerAvatars.Length; i++)
            {
                _availableAvatarIndices.Add(i);
            }

            // remove the already taken avatars
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values.Where(player =>
                         player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber))
            {
                _availableAvatarIndices.Remove(PlayerData.Read<int>(player,
                    GameConstants.CustomPlayerProperties.AvatarIndex));
            }

            // find a non taken avatar and remove it from the list of available avatars
            var avatarIndex = _availableAvatarIndices[Random.Range(0, _availableAvatarIndices.Count)];
            _availableAvatarIndices.Remove(avatarIndex);
            _selfAvatarChoice = avatarIndex;

            var p = PhotonNetwork.LocalPlayer;
            PlayerData.Set(p, GameConstants.CustomPlayerProperties.AvatarIndex, avatarIndex);
            PlayerData.Set(p, GameConstants.CustomPlayerProperties.IsHost, _isHost);
        }

        private void Start()
        {
            startingCountDown.gameObject.SetActive(false);
            
            // all slots is a copy of available player slots, used to restore slots when players leave
            _allPlayerSlots = new List<PlayerLobbyItem>(availablePlayerSlots);

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
                    data = new PlayerLobbyItemData(player, _selfAvatarChoice);
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
            if (_isHost)
            {
                hostCrown.GetComponent<RectTransform>().SetPositionAndRotation(_selfSlot.GetCrownLocation(), _selfSlot.GetCrownRotation());
            }
            else
            {
                RefreshCrown();
            }

            photonView.RPC(nameof(JoinGame), RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer.ActorNumber, _selfAvatarChoice);
        }

        public void StartGame()
        {
            photonView.RPC(nameof(StartGameRPC), RpcTarget.AllBufferedViaServer);
        }
        
        public void CancelStartGame()
        {
            photonView.RPC(nameof(CancelStartGameRPC), RpcTarget.AllBufferedViaServer);
        }

        private void CheckObjectsAvailability()
        {
            if (_isHost)
            {
                foreach (var obj in playerObjects)
                {
                    obj.SetActive(false);
                }

                foreach (var obj in hostObjects)
                {
                    obj.SetActive(true);
                }

                startGameButton.interactable = LobbyData.Instance.PartyGame.minimumPlayers == 0 ||
                                               LobbyData.Instance.Players.Count >=
                                               LobbyData.Instance.PartyGame.minimumPlayers;
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
            
            cancelButton.SetActive(false);
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
            if (_isHost)
            {
                startGameButton.gameObject.SetActive(false);
                cancelButton.SetActive(true);
            }
            
            StartCoroutine(StartGameRoutine());
            
            IEnumerator StartGameRoutine()
            {
                SoundManager.Play(GameConstants.Sounds.Countdown);
                startingCountDown.text = "3";
                startingCountDown.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                
                SoundManager.Play(GameConstants.Sounds.Countdown);
                startingCountDown.text = "2";
                yield return new WaitForSeconds(1f);
                
                SoundManager.Play(GameConstants.Sounds.Countdown);
                startingCountDown.text = "1";
                yield return new WaitForSeconds(1f);
                
                SceneManager.TransitionToScene(LobbyData.Instance.PartyGame.gameScene);
            }
        }

        [PunRPC]
        private void CancelStartGameRPC()
        {
            startingCountDown.gameObject.SetActive(false);

            if (_isHost)
            {
                startGameButton.gameObject.SetActive(true);
                cancelButton.SetActive(false);
            }
            
            StopAllCoroutines();
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
                    _isHost = true;
                    GlobalData.Set(GameConstants.GlobalDataKeys.IsHost, true);
                    PlayerData.Set(PhotonNetwork.LocalPlayer, GameConstants.CustomPlayerProperties.IsHost, true);

                    hostCrown.GetComponent<RectTransform>().position = _selfSlot.GetCrownLocation();
                    hostCrown.GetComponent<RectTransform>().rotation = _selfSlot.GetCrownRotation();
                }
            }

            CheckObjectsAvailability();

            if (!_isHost)
            {
                RefreshCrown();
            }
        }

        private void RestorePlayerSlot(Player player)
        {
            var data = LobbyData.Instance.Players.Find(p => p.actorID == player.ActorNumber);
            var slot = _allPlayerSlots[data.slotTaken];
            slot.ShrinkIcon(() =>
            {
                slot.gameObject.SetActive(false);
                availablePlayerSlots[data.slotTaken] = slot;
            });
        }

        private void AddPlayerToNextAvailableSlot(PlayerLobbyItemData data)
        {
            var placeHolderSlot = availablePlayerSlots.First(element => element is not null);
            data.slotTaken = _allPlayerSlots.IndexOf(placeHolderSlot);
            availablePlayerSlots[data.slotTaken] = null;
            
            var placeHolderSlotTransform = placeHolderSlot.transform;
            var actualSlot =
                Instantiate(LobbyData.Instance.PartyGame.playerAvatars[data.avatarIndex].PlayerLobbyItemPrefab,
                    placeHolderSlotTransform.position, placeHolderSlotTransform.rotation,
                    placeHolderSlotTransform.parent);
            
            _allPlayerSlots[data.slotTaken] = actualSlot;
            
            Destroy(placeHolderSlot.gameObject);
      
            actualSlot.ZoomIcon(Vector3.zero);
            actualSlot.data = data;
            actualSlot.UpdateAppearance();

            if (data.actorID == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _selfSlot = actualSlot;
            }
        }

        private void RefreshCrown()
        {
            foreach (var slot in _allPlayerSlots)
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
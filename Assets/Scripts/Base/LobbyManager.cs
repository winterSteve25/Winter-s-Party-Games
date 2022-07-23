using System.Collections.Generic;
using System.Linq;
using Games.Base;
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
        
        [SerializeField] protected PartyGame gameMode;

        protected List<PlayerLobbyItemData> Players;
        protected bool IsHost;
        
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
        }

        protected virtual void Start()
        {
            roomCodeText.text += PhotonNetwork.CurrentRoom.Name;
            Players = PhotonNetwork.CurrentRoom.Players.Select(pair => new PlayerLobbyItemData(pair.Value)).ToList();
            CheckStartButtonAvailability();            
        }

        public virtual void StartGame()
        {
            SceneTransition.TransitionToScene(gameMode.gameScene);
        }

        protected virtual void CheckStartButtonAvailability()
        {
            if (IsHost)
            {
                startGameButton.interactable = gameMode.minimumPlayers == 0 || Players.Count >= gameMode.minimumPlayers;
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Players.Add(new PlayerLobbyItemData(newPlayer));   
            CheckStartButtonAvailability();
        }
    }
}
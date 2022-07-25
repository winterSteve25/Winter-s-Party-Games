using Games.Base;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Network.Scenes
{
    public class CreateRoomButton : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PartyGame partyGame;
        private bool _creating;
        
        public override void OnEnable()
        {
            base.OnEnable();
            GetComponent<Button>().onClick.AddListener(CreateRoomButtonClicked);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            GetComponent<Button>().onClick.RemoveListener(CreateRoomButtonClicked);
        }

        private void CreateRoomButtonClicked()
        {
            GetComponent<Button>().interactable = false;
            GlobalData.Set(GameConstants.GlobalData.IsHost, true);
            CreateRoom();
        }

        private void CreateRoom()
        {
            var s = Random.Range(0, 1000000).ToString("D6");

            PhotonNetwork.CreateRoom(s, new RoomOptions
            {
                CustomRoomProperties = partyGame.CustomRoomProperties,
                MaxPlayers = partyGame.maximumPlayers,
            });
        }

        public override void OnJoinedRoom()
        {
            SceneTransition.TransitionToScene(partyGame.roomScene);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (returnCode == ErrorCode.GameIdAlreadyExists)
            {
                CreateRoom();
            }
            else
            {
                GetComponent<Button>().interactable = true;
            }
        }
    }
}
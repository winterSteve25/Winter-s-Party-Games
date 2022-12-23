using Games.Base;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.Data;
using Random = UnityEngine.Random;

namespace Network.Scenes
{
    public class CreateRoomButton : MonoBehaviourPunCallbacks, IPointerClickHandler
    {
        [SerializeField, Required] private PartyGame partyGame;
        [SerializeField, Required] private Image gameModePreview;
        [SerializeField, Required] private TMP_Text gameModeName;

        private bool _creating;

        private void Start()
        {
            gameModePreview.sprite = partyGame.gameModePreview;
            gameModeName.text = partyGame.gameModeName;
        }

        private void CreateRoom()
        {
            _creating = true;
            
            var s = Random.Range(0, 1000000).ToString("D6");
            PhotonNetwork.CreateRoom(s, new RoomOptions
            {
                CustomRoomProperties = partyGame.CustomRoomProperties,
                MaxPlayers = partyGame.maximumPlayers,
            });
        }

        public override void OnJoinedRoom()
        {
            _creating = false;
            SceneManager.TransitionToScene(partyGame.roomScene);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (returnCode == ErrorCode.GameIdAlreadyExists)
            {
                CreateRoom();
            }
            else
            {
                _creating = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_creating) return;
            GlobalData.Set(GameConstants.GlobalDataKeys.IsHost, true);
            CreateRoom();
        }
    }
}
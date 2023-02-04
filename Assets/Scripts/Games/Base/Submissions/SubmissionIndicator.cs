using Base;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Base.Submissions
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class SubmissionIndicator : MonoBehaviourPunCallbacks
    {
        private PlayerLobbyItem[] _slots;
        private PhotonView _photonView;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            
            var playerLobbyItemDatas = LobbyData.Instance.Players;
            var count = playerLobbyItemDatas.Count;
            _slots = new PlayerLobbyItem[count];

            for (var i = 0; i < count; i++)
            {
                var data = playerLobbyItemDatas[i];
                var playerLobbyItem = Instantiate(LobbyData.Instance.PartyGame.playerAvatars[data.avatarIndex].PlayerLobbyItemPrefab, transform);
                playerLobbyItem.data = data;
                playerLobbyItem.UpdateAppearance();
                playerLobbyItem.ChangeColor(new Color(0.4f, 0.4f, 0.4f));
                playerLobbyItem.ShrinkIcon(endScale: 0.6f);
                _slots[i] = playerLobbyItem;
            }
        }

        [PunRPC]
        private void Submit(int actorID)
        {
            // animate the player slot visual to indicate player has finished
            var data = LobbyData.Instance.Players.Find(data => data.actorID == actorID);
            var playerLobbyItem = _slots[data.slotTaken];
            playerLobbyItem.ChangeColor(Color.white);
            playerLobbyItem.ZoomIcon();
        }

        public void Submit()
        {
            _photonView.RPC(nameof(Submit), RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            var data = LobbyData.Instance.Players.Find(data => data.actorID == otherPlayer.ActorNumber);
            _slots[data.slotTaken].Destroy();
            _slots[data.slotTaken] = null;
        }
    }
}
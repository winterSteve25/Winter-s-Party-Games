using Base;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Base
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class SubmissionIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        private PlayerLobbyItem[] _slots;

        private void Start()
        {
            var playerLobbyItemDatas = LobbyData.Instance.Players;
            var count = playerLobbyItemDatas.Count;
            _slots = new PlayerLobbyItem[count];

            for (var i = 0; i < count; i++)
            {
                var go = Instantiate(slotPrefab, transform);
                go.GetComponentInChildren<Image>().color = new Color(0.4f, 0.4f, 0.4f);
                _slots[i] = go.GetComponent<PlayerLobbyItem>();
            }
            
            foreach (var data in playerLobbyItemDatas)
            {
                var playerLobbyItem = _slots[data.slotTaken];
                playerLobbyItem.data = data;
                playerLobbyItem.UpdateAppearance();
            }
        }

        public TweenerCore<Vector3, Vector3, VectorOptions> Submit(int actorID)
        {
            // animate the player slot visual to indicate player has finished
            var data = LobbyData.Instance.Players.Find(data => data.actorID == actorID);
            var playerLobbyItem = _slots[data.slotTaken];
            playerLobbyItem.GetComponentInChildren<Image>().color = Color.white;
            var rectTransform = playerLobbyItem.GetComponent<RectTransform>();
            return rectTransform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBounce);
        }
    }
}
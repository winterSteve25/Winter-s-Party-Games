using Base;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Games.Base
{
    public class VoteIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;

        public TweenerCore<Vector3, Vector3, VectorOptions> Vote(int actorID, Vector2 voteOptionIndicatorPosition, Quaternion voteOptionIndicatorRotaition)
        {
            var data = LobbyData.Instance.Players.Find(data => data.actorID == actorID);
            var playerLobbyItem = Instantiate(slotPrefab, transform);
            var lobbyItem = playerLobbyItem.GetComponent<PlayerLobbyItem>();
            lobbyItem.data = data;
            lobbyItem.UpdateAppearance();
            var rectTransform = playerLobbyItem.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.zero;
            rectTransform.position = voteOptionIndicatorPosition + Random.insideUnitCircle;
            rectTransform.rotation = voteOptionIndicatorRotaition * Quaternion.Euler(0, 0, Random.Range(0, 0.1f));
            return rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Games.ScrambledEggs.Procedure
{
    public class ScrambledEggsWinner : MonoBehaviour
    {
        [SerializeField] private GameObject playerItemPrefab;
        [SerializeField] private Transform row;
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject tieMessage;

        private IEnumerator Start()
        {
            tieMessage.SetActive(false);
            yield return new WaitForSeconds(1f);
            var scores = GlobalData.Read<Dictionary<int, int>>(GameConstants.GlobalData.LatestScoring);
            var data = LobbyData.Instance.Players;

            var sorted = (from entry in scores orderby entry.Value descending select entry).ToList();
            var highestScore = sorted.First().Value;
            var winners = sorted.Where(element => element.Value == highestScore).ToArray();

            foreach (var (actorId, score) in winners)
            {
                var playerItem = Instantiate(playerItemPrefab, row);
                playerItem.gameObject.SetActive(false);
                var component = playerItem.GetComponent<RectTransform>();
                component.localScale = Vector3.zero;
                playerItem.gameObject.SetActive(true);
                playerItem.GetComponent<PlayerLobbyItem>().data = data.Find(element => element.actorID == actorId);
                playerItem.GetComponent<PlayerLobbyItem>().UpdateAppearance();
                component.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f);
            }

            if (winners.Length != 1)
            {
                tieMessage.SetActive(true);
                tieMessage.GetComponent<CanvasGroup>().DOFade(1, 1f);
            }

            yield return new WaitForSeconds(2f);
            backButton.SetActive(true);
            backButton.GetComponent<CanvasGroup>().DOFade(1, 0.6f).SetEase(Ease.InOutCubic);
        }
    }
}
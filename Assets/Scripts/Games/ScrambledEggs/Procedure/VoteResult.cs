using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using DG.Tweening;
using Games.Base;
using Games.ScrambledEggs.Data;
using Photon.Pun;
using UnityEngine;
using Utils;
using Button = UnityEngine.UI.Button;

namespace Games.ScrambledEggs.Procedure
{
    public class VoteResult : MonoBehaviour
    {
        [SerializeField] private Stage5SubmissionVote mostLiked;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        private IEnumerator Start()
        {
            mostLiked.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            var rectTransform = mostLiked.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.zero;
            mostLiked.gameObject.SetActive(true);
            mostLiked.GetComponent<Button>().interactable = false;
            var votes = GlobalData.Read<List<Vote>>(GameConstants.GlobalData.ScrambledEggsVoteResult);
            var index = votes.MostVoted();
            var submissions = GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData).Stage5Submissions;
            var paintingSubmission = submissions[index];
            mostLiked.Init(index, paintingSubmission.SubmissionContent, paintingSubmission.Context);
            rectTransform.DOScale(new Vector3(1, 1, 1), 1f);
        }

        private void OnDestroy()
        {
            Destroy(LobbyData.Instance.gameObject);
        }
    }
}
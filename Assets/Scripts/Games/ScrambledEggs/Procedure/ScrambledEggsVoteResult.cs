using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Games.Base.Votes;
using Games.ScrambledEggs.Data;
using Photon.Pun;
using UnityEngine;
using Utils;
using Button = UnityEngine.UI.Button;

namespace Games.ScrambledEggs.Procedure
{
    public class ScrambledEggsVoteResult : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform row;
        [SerializeField] private GameObject tieMessage;
        
        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        private IEnumerator Start()
        {
            tieMessage.SetActive(false);
            yield return new WaitForSeconds(1f);
            var votes = GlobalData.Read<List<Vote>>(GameConstants.GlobalData.LatestVoteResult);
            var indices = votes.SortByVoteCount();
            var data = GlobalData.Read<ScrambledEggsGameData>(GameConstants.GlobalData.LatestGameData);
            var submissions = data.GetDrawingTask(data.GetRoundOn() - 1);

            var mostVoted = indices.First();
            var tops = indices.Where(kv => kv.Value == mostVoted.Value).ToArray();
            
            foreach (var (index, voteCount) in tops)
            {
                var showcase = Instantiate(prefab, row);
                showcase.gameObject.SetActive(false);
                var component = showcase.GetComponent<RectTransform>();
                component.localScale = Vector3.zero;
                showcase.gameObject.SetActive(true);
                showcase.GetComponent<Button>().interactable = false;
                var submission = submissions[index];
                showcase.GetComponent<ScrambledEggsPaintingSubmissionVote>().Init(index, submission.SubmissionContent, submission.Context);
                component.DOScale(new Vector3(1, 1, 1), 1f);
            }

            if (tops.Length > 1)
            {
                tieMessage.SetActive(true);
                tieMessage.GetComponent<CanvasGroup>().DOFade(1, 1f);
            }

            yield return new WaitForSeconds(4f);
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.ScrambledEggsOfDoomScoreboard);
        }
    }
}
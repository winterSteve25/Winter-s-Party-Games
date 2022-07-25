using System;
using Games.ScrambledEggs.Data;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Games.ScrambledEggs.Procedure
{
    public class VotePopulater : MonoBehaviour
    {
        [SerializeField] private GameObject submissionPrefab;
        [SerializeField] private Transform row1;
        [SerializeField] private Transform row2;
        
        private void Start()
        {
            var data = GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData);
            var submissions = data.Stage5Submissions;
            submissions.Sort((a, b) => b.SubmitterActorID.CompareTo(a.SubmitterActorID));

            var submissionsCount = submissions.Count % 4;
            var perRow = submissionsCount == 0 ? 4 : submissionsCount;

            var k = 0;
            
            for (var i = 0; i < submissions.Count * 0.25f; i++)
            {
                for (var j = 0; j < perRow; j++)
                {
                    var sub = submissions[k];
                    
                    var instantiate = Instantiate(submissionPrefab, i == 0 ? row1 : row2);
                    instantiate.GetComponent<Stage5SubmissionVote>().Init(k, sub.SubmissionContent, sub.Context);
                    if (sub.SubmitterActorID == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        instantiate.GetComponent<Button>().interactable = false;
                    }
                    k++;
                }
            }
        }
    }
}
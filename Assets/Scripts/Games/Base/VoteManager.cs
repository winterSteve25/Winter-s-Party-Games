using System.Collections.Generic;
using System.Linq;
using Base;
using Games.Utils;
using Photon.Pun;
using Settings;
using UnityEngine;
using Utils;

namespace Games.Base
{
    public class VoteManager : MonoBehaviour
    {
        [SerializeField] private int allowedVotesPerPerson;
        [SerializeField] private Timer timer;
        [SerializeField] private ScrambledEggsOfDoomSettings settings;
        [SerializeField] private SubmissionIndicator submissionIndicator;
        
        [SerializeField] private GameObject inputs;
        [SerializeField] private GameObject waitingText;

        private int _totalVotesCount;
        private List<Vote> _votes;
        private bool _ended;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            timer.timeLimit = settings.timeLimitVoting;
            timer.StartTimer();
        }

        private void Start()
        {
            waitingText.SetActive(false);
            _totalVotesCount = PhotonNetwork.CurrentRoom.PlayerCount;
            _votes = new List<Vote>();
        }
        
        private void OnEnable()
        {
            timer.onComplete.AddListener(EndVote);
        }

        private void OnDisable()
        {
            timer.onComplete.RemoveListener(EndVote);
        }

        public void EndVote()
        {
            _ended = true;
            GlobalData.Set(GameConstants.GlobalData.ScrambledEggsVoteResult, _votes);
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(LobbyData.Instance.gameMode.endScene);
        }

        public void Vote(int voter, int votedForIndex)
        {
            PhotonView.Get(this).RPC(nameof(VoteRPC), RpcTarget.All, voter, votedForIndex);
        }

        [PunRPC]
        private void VoteRPC(int voter, int votedForIndex)
        {
            if (_ended) return;
            if (_votes.Count(vote => vote.Voter == voter) > allowedVotesPerPerson) return;
            if (submissionIndicator != null)
            {
                submissionIndicator.Submit(voter);
            }
            _votes.Add(new Vote(voter, votedForIndex));
            if (_votes.Count(vote => vote.Voter == voter) <= allowedVotesPerPerson) return;
            
            inputs.SetActive(false);
            waitingText.SetActive(true);
        }
    }
}
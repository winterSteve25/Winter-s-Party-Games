using System.Collections.Generic;
using System.Linq;
using Base;
using DG.Tweening;
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
        [SerializeField] private VoteIndicator voteIndicator;

        private List<Vote> _votes;
        private bool _ended;
        private int _selfVotedCounts;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            timer.timeLimit = settings.timeLimitVoting;
            timer.StartTimer();
        }

        private void Start()
        {
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

        private void EndVote()
        {
            _ended = true;
            GlobalData.Set(GameConstants.GlobalData.ScrambledEggsVoteResult, _votes);
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(LobbyData.Instance.gameMode.endScene);
        }

        public void Vote(int voter, int votedForIndex, Vector2 voteOptionIndicatorPosition, Quaternion voteOptionIndicatorRotation)
        {
            if (_selfVotedCounts >= allowedVotesPerPerson) return;
            _selfVotedCounts++;
            PhotonView.Get(this).RPC(nameof(VoteRPC), RpcTarget.All, voter, votedForIndex, voteOptionIndicatorPosition, voteOptionIndicatorRotation);
        }

        private int _dataReceived;

        [PunRPC]
        private void VoteRPC(int voter, int votedForIndex, Vector2 voteOptionIndicatorPosition, Quaternion voteOptionIndicatorRotation)
        {
            if (_ended) return;
            _votes.Add(new Vote(voter, votedForIndex));
            
            _dataReceived++;
            
            voteIndicator.Vote(voter, voteOptionIndicatorPosition, voteOptionIndicatorRotation).OnComplete(() =>
            {
                if (_dataReceived == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    EndVote();
                }
            });
        }
    }
}
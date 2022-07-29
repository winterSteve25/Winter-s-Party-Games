using System;
using System.Collections.Generic;
using Base;
using DG.Tweening;
using Games.Utils;
using Network;
using Photon.Pun;
using Settings;
using UnityEngine;
using Utils;

namespace Games.Base.Votes
{
    public class VoteManager : MonoBehaviour
    {
        public static event Action<List<Vote>> VoteEnded;
        
        [SerializeField] private int allowedVotesPerPerson;
        [SerializeField] private Timer timer;
        [SerializeField] private ScrambledEggsOfDoomSettings settings;
        [SerializeField] private VoteIndicator voteIndicator;

        [SerializeField] private bool overrideTransitionScene;
        [SerializeField] private GameConstants.SceneIndices overrideValue;

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
            GlobalData.Set(GameConstants.GlobalData.LatestVoteResult, _votes);
            VoteEnded?.Invoke(_votes);
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(overrideTransitionScene
                ? overrideValue
                : LobbyData.Instance.gameMode.voteResultScene);
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
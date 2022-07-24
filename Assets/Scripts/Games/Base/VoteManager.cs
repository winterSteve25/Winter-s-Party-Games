using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Games.Base
{
    public class VoteManager : MonoBehaviour
    {
        [SerializeField] private int allowedVotesPerPerson;
        
        private int _totalVotesCount;
        private List<Vote> _votes;
        private bool _ended;
        
        private void Start()
        {
            _totalVotesCount = PhotonNetwork.CurrentRoom.PlayerCount;
            _votes = new List<Vote>();
        }

        public void EndVote()
        {
            _ended = true;
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
            _votes.Add(new Vote(voter, votedForIndex));
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Games.Base.Votes
{
    public class Vote
    {
        public int Voter;
        public int VotedForIndex;

        public Vote(int voter, int votedForIndex)
        {
            Voter = voter;
            VotedForIndex = votedForIndex;
        }

        public override string ToString()
        {
            return $"{{Voter: {Voter}, Voted for: {VotedForIndex}}}";
        }
    }

    public class CachedVote : Vote
    {
        public readonly Vector2 VoteOptionIndicatorPosition;
        public readonly Quaternion VoteOptionIndicatorRotation;
        
        public CachedVote(int voter, int votedForIndex, Vector2 voteOptionIndicatorPosition, Quaternion voteOptionIndicatorRotation) : base(voter, votedForIndex)
        {
            VoteOptionIndicatorPosition = voteOptionIndicatorPosition;
            VoteOptionIndicatorRotation = voteOptionIndicatorRotation;
        }
    }

    public static class VoteExtension
    {
        public static List<KeyValuePair<int, int>> SortByVoteCount(this IEnumerable<Vote> votes)
        {
            // keys are vote index, values are how many voted for
            var voteCounts = new Dictionary<int, int>();

            foreach (var voteVotedForIndex in votes.Select(vote => vote.VotedForIndex))
            {
                if (voteCounts.ContainsKey(voteVotedForIndex))
                {
                    voteCounts[voteVotedForIndex]++;
                }
                else
                {
                    voteCounts.Add(voteVotedForIndex, 1);
                }
            }

            return (from entry in voteCounts orderby entry.Value descending select entry).ToList();
        }
    }
}
using System.Collections.Generic;
using System.Linq;

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

    public static class VoteExtension
    {
        public static List<KeyValuePair<int, int>> SortByVoteCount(this IEnumerable<Vote> votes, out int tiesCount)
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

            var sorted = (from entry in voteCounts orderby entry.Value descending select entry).ToList();
            var mostVotes = sorted.First().Value;
            
            tiesCount = sorted.Where((kv, index) => index != 0 && kv.Value == mostVotes).Count();
            
            return sorted;
        }
    }
}
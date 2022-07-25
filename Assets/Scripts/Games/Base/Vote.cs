using System.Collections.Generic;
using System.Linq;

namespace Games.Base
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
        public static int MostVoted(this IEnumerable<Vote> votes, out List<int> ties)
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

            ties = new List<int>();
            var highest = (-1, 0);

            foreach (var (k, v) in voteCounts)
            {
                if (highest.Item1 == -1)
                {
                    highest = (k, v);
                    continue;
                }

                if (v == highest.Item2)
                {
                    ties.Add(v);
                    continue;
                }

                if (v > highest.Item2)
                {
                    ties.Clear();
                    highest = (k, v);
                }
            }

            return highest.Item1;
        }
    }
}
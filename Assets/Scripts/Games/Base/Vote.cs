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
}
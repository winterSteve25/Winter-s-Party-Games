using System.Linq;

namespace Games.GameModes.WordMania.VoteManagement
{
    public class WordManiaVote
    {
        public readonly int VoterActorId;
        public readonly int VoterVotedPromptIndex;
        public readonly int VoterVotedForIndex;

        public WordManiaVote(int voterActorId, int voterVotedPromptIndex, int voterVotedForIndex)
        {
            VoterActorId = voterActorId;
            VoterVotedPromptIndex = voterVotedPromptIndex;
            VoterVotedForIndex = voterVotedForIndex;
        }

        public static object Serialize(WordManiaVote vote)
        {
            return $"{vote.VoterActorId}|{vote.VoterVotedPromptIndex}|{vote.VoterVotedForIndex}";
        }

        public static WordManiaVote Deserialize(object data)
        {
            var content = ((string)data).Split("|").Select(int.Parse).ToArray();
            return new WordManiaVote(content[0], content[1], content[2]);
        }
    }
}
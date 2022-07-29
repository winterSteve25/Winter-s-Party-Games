using System.Collections.Generic;
using System.Linq;
using Games.Base.Votes;
using Games.ScrambledEggs.Data;
using UnityEngine;
using Utils;

namespace Games.Base.Scoreboard
{
    public class ScoreSetter : MonoBehaviour
    {
        private void OnEnable()
        {
            VoteManager.VoteEnded += Set;
        }

        private void OnDisable()
        {
            VoteManager.VoteEnded -= Set;
        }

        private void Set(List<Vote> votes)
        {
            var indices = votes.SortByVoteCount(out _);

            var data = GlobalData.Read<ScrambledEggsGameData>(GameConstants.GlobalData.LatestGameData);
            var submissions = data.GetDrawingTask(data.GetRoundOn() - 1);

            var scores = new Dictionary<int, int>();
            var previous = GlobalData.HasKey(GameConstants.GlobalData.PreviousScoring) ? GlobalData.Read<Dictionary<int, int>>(GameConstants.GlobalData.LatestScoring) : new Dictionary<int, int>();

            var isNew = !previous.Any();
            
            foreach (var (index, voteCount) in indices)
            {
                var submission = submissions[index];
                var submissionSubmitterActorID = submission.SubmitterActorID;
                
                if (isNew)
                {
                    previous.Add(submissionSubmitterActorID, 0);
                }
                
                scores.Add(submissionSubmitterActorID, previous[submissionSubmitterActorID] + voteCount * 100);
            }

            GlobalData.Set(GameConstants.GlobalData.PreviousScoring, previous);
            GlobalData.Set(GameConstants.GlobalData.LatestScoring, scores);
        }
    }
}
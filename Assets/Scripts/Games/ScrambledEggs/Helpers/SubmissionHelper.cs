using System.Collections.Generic;
using Games.Base;
using Games.ScrambledEggs.Data;
using Photon.Pun;
using Photon.Realtime;
using Utils;

namespace Games.ScrambledEggs.Helpers
{
    public static class SubmissionHelper
    {
        public static Submission<T> FindSubmission<T>(List<Submission<T>> submissions, Player player, T botSubmission)
        {
            submissions.Sort((a, b) => b.SubmitterActorID.CompareTo(a.SubmitterActorID));

            // find the submission in front of this player's
            var mySubmission = submissions.Find(submission => submission.SubmitterActorID == player.ActorNumber);
            var frontIndex = submissions.IndexOf(mySubmission) - 1;

            if (frontIndex == -1)
            {
                // if there is an even number of players, the first player get the last player's submission
                if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0)
                {
                    frontIndex = submissions.Count - 1;
                }
                // if there is an odd number of players, the first player get a bot submission
                else
                {
                    return new Submission<T>(GameConstants.Flags.BotPlayerActorID, botSubmission);
                }
            }

            return submissions[frontIndex];
        }
    }
}
using Photon.Pun;

namespace Games.Base.Submissions
{
    public class Submission<T>
    {
        public int SubmitterActorID;
        public T SubmissionContent;

        public Submission(int submitterActorID, T submissionContent)
        {
            SubmitterActorID = submitterActorID;
            SubmissionContent = submissionContent;
        }

        public Submission(T submissionContent) : this(PhotonNetwork.LocalPlayer.ActorNumber, submissionContent)
        {
        }

        public override string ToString()
        {
            return $"{{Actor ID: {SubmitterActorID}, Submission Content: {SubmissionContent.ToString()}}}";
        }
    }
}
namespace Games.ScrambledEggs.Data
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
    }
}
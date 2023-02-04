namespace Games.Base.Submissions
{
    public class StringSubmission : Submission<string>
    {
        public StringSubmission(int submitterActorID, string submissionContent) : base(submitterActorID, submissionContent)
        {
        }

        public StringSubmission(string submissionContent) : base(submissionContent)
        {
        }
    }
}
using System;

namespace Games.Base.Submissions
{
    public class IntegerSubmissionManager : SubmissionManager<int>
    {
        private static readonly Deserializer SubmissionDeserializer = data =>
        {
            var content = ((string)data).Split('|');
            return new IntegerSubmission(int.Parse(content[0]), int.Parse(content[1]));
        };
        
        public IntegerSubmissionManager(Action onComplete) : base(SubmissionDeserializer, onComplete)
        {
        }
    }
}
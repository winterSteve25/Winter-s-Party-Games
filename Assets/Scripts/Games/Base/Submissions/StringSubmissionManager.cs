using System;

namespace Games.Base.Submissions
{
    public class StringSubmissionManager : SubmissionManager<string>
    {
        private static readonly Deserializer SubmissionDeserializer = data =>
        {
            var content = (string)data;
            var firstIndex = content.IndexOf('|');
            return new StringSubmission(int.Parse(content[..firstIndex]), content[(firstIndex+1)..]);
        };

        public StringSubmissionManager(Action onComplete) : base(SubmissionDeserializer, onComplete)
        {
        }
    }
}
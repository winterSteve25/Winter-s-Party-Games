using System;
using Games.Base.Submissions;

namespace Games.GameModes.WordMania.PromptManagement
{
    public class WordManiaPromptSubmissionManager : SubmissionManager<WordManiaPromptSubmission>
    {
        public WordManiaPromptSubmissionManager(Action onComplete) : base(
            sub =>
            {
                var content = sub.SubmissionContent;
                return $"{sub.SubmitterActorID}|{content.Prompt1}|{content.Prompt2}|{content.FirstAnswer}|{content.SecondAnswer}";
            },
            obj =>
            {
                var content = ((string)obj).Split("|");
                return new Submission<WordManiaPromptSubmission>(
                    int.Parse(content[0]),
                    new WordManiaPromptSubmission(
                        int.Parse(content[1]),
                        int.Parse(content[2]),
                        content[3],
                        content[4]
                    ));
            },
            onComplete
        )
        {
        }
    }
}
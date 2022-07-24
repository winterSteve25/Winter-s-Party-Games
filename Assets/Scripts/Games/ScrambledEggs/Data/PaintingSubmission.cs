using UnityEngine;

namespace Games.ScrambledEggs.Data
{
    public class PaintingSubmission : Submission<Texture2D>
    {
        public string Context;
        
        public PaintingSubmission(int submitterActorID, Texture2D submissionContent, string context) : base(submitterActorID, submissionContent)
        {
            Context = context;
        }
    }
}
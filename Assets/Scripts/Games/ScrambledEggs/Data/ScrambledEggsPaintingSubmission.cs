using Games.Base;
using UnityEngine;

namespace Games.ScrambledEggs.Data
{
    public class ScrambledEggsPaintingSubmission : Submission<Texture2D>
    {
        public string Context;
        
        public ScrambledEggsPaintingSubmission(int submitterActorID, Texture2D submissionContent, string context) : base(submitterActorID, submissionContent)
        {
            Context = context;
        }
    }
}
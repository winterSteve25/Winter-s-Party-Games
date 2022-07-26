using System;
using UnityEngine;
using Utils.Dictionary;

namespace Games.ScrambledEggs.Data
{
    [Serializable]
    public struct ScrambledEggsPromptPart
    {
        public ScrambledEggsPromptPartTypes type;
        
        [Tooltip("This will be used when type is Constant Word")]
        public string constantWord;

        [Tooltip("This will be used when type is Submission")]
        public int submissionFromIndex;
        
        [Tooltip("This will be used when type is Submission as a backup answer if submission was empty or when type is Random Word")]
        public PartOfSpeech generateRandom;
    }

    public enum ScrambledEggsPromptPartTypes
    {
        ConstantWord,
        Submission,
        RandomWord
    }
}
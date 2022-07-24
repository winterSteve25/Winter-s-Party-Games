using UnityEngine;
using UnityEngine.Serialization;

namespace Settings
{
    [CreateAssetMenu(fileName = "Scrambled Eggs of Doom Settings", menuName = "Winter's Party Games/Settings/Scrambled Eggs of Doom", order = 0)]
    public class ScrambledEggsOfDoomSettings : ScriptableObject
    {
        [FormerlySerializedAs("timeLimitSimple")] [Tooltip("Time players has to finish their simple tasks (stage 1-4)")]
        public int timeLimitWord = 30;
        
        [FormerlySerializedAs("timeLimitComplex")] [Tooltip("Time players has to finish their complex tasks (stage 5)")]
        public int timeLimitDrawing = 60;
    }
}
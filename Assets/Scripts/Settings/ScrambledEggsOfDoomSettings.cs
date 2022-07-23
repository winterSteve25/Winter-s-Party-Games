using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Scrambled Eggs of Doom Settings", menuName = "Winter's Party Games/Settings/Scrambled Eggs of Doom", order = 0)]
    public class ScrambledEggsOfDoomSettings : ScriptableObject
    {
        [Tooltip("Time players has to finish their simple tasks (stage 1-4)")]
        public int timeLimitSimple = 30;
        
        [Tooltip("Time players has to finish their complex tasks (stage 5)")]
        public int timeLimitComplex = 60;
    }
}
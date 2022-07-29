using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Scrambled Eggs of Doom Settings", menuName = "Winter's Party Games/Settings/Scrambled Eggs of Doom", order = 0)]
    public class ScrambledEggsOfDoomSettings : ScriptableObject
    {
        [Tooltip("Time players has to finish their word tasks")]
        public int timeLimitWord = 30;
        
        [Tooltip("Time players has to finish their drawing task")]
        public int timeLimitDrawing = 60;
        
        [Tooltip("Time players has to finish their voting")]
        public int timeLimitVoting = 60;
    }
}
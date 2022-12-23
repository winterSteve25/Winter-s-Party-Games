using KevinCastejon.MoreAttributes;
using UnityEngine;

namespace Utils.Data
{
    [CreateAssetMenu(fileName = "Scene Data Instance", menuName = "Winter's Party Games/Scene Data Instance")]
    public class SceneData : ScriptableObject
    {
        [Scene] public string mainMenu;
        [Scene] public string createOrJoin;
        [Scene] public string preferences;
        [Scene] public string joinViaSteam;
        [Scene] public string joinRoom;
        [Scene] public string createRoom;
        [Scene] public string joiningRoom;
    }
}
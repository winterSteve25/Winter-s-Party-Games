using TMPro;
using UnityEngine;
using Utils;
using Utils.Data;

namespace Network.Scenes
{
    public class JoinRoomButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField joinInput;
        
        public void JoinRoomButtonClicked()
        {
            if (string.IsNullOrEmpty(joinInput.text)) return;
            GlobalData.Set(GameConstants.GlobalDataKeys.IsHost, false);
            GlobalData.Set(GameConstants.GlobalDataKeys.RoomIDToJoin, joinInput.text.Replace("\r", "").Replace(" ", ""));
            SceneManager.TransitionToScene(GameConstants.SceneData.joiningRoom);
        }
    }
}
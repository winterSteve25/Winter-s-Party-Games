using UnityEngine;
using Utils;
using Utils.Data;

namespace Network.Scenes
{
    public class CreateOrJoinRoom : MonoBehaviour
    {
        public void Create()
        {
            SceneManager.TransitionToScene(GameConstants.SceneData.createRoom);
        }

        public void Join()
        {
            SceneManager.TransitionToScene(GameConstants.SceneData.joinRoom);
        }
    }
}
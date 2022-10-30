using UnityEngine;
using Utils;

namespace Network.Scenes
{
    public class CreateOrJoinRoom : MonoBehaviour
    {
        public void Create()
        {
            SceneManager.TransitionToScene(GameConstants.SceneIndices.CreateRoom);
        }

        public void Join()
        {
            SceneManager.TransitionToScene(GameConstants.SceneIndices.JoinRoom);
        }
    }
}
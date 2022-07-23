using UnityEngine;
using Utils;

namespace Network.Scenes
{
    public class CreateOrJoinRoom : MonoBehaviour
    {
        public void Create()
        {
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.CreateRoom);
        }

        public void Join()
        {
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.JoinRoom);
        }
    }
}
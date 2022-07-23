using UnityEngine;
using Utils;

namespace Menu
{
    public class MenuButtons : MonoBehaviour
    {
        public void PlayButtonClicked()
        {
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.CreateOrJoinRoom);
        }

        public void PreferencesButtonClicked()
        {
            
        }

        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
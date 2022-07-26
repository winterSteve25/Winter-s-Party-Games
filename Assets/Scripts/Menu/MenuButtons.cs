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
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.Preferences);
        }

        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
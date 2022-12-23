using UnityEngine;
using Utils;
using Utils.Audio;
using Utils.Data;

namespace Menu
{
    public class MenuButtons : MonoBehaviour
    {
        private void Start()
        {
            SoundManager.Play(GameConstants.Sounds.Tune1);
        }

        public void PlayButtonClicked()
        {
            SceneManager.TransitionToScene(GameConstants.SceneData.createOrJoin);
        }

        public void PreferencesButtonClicked()
        {
            SceneManager.TransitionToScene(GameConstants.SceneData.preferences);
        }

        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
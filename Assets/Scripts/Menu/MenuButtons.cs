using System;
using UnityEngine;
using Utils;
using Utils.Audio;

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
            SceneManager.TransitionToScene(GameConstants.SceneIndices.CreateOrJoinRoom);
        }

        public void PreferencesButtonClicked()
        {
            SceneManager.TransitionToScene(GameConstants.SceneIndices.Preferences);
        }

        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
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
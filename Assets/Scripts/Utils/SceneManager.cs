using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using Utils.Audio;
using Utils.Data;

namespace Utils
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager _instance;

        [SerializeField] private float transitionTime = 1;
        [SerializeField] private Animator animator;

        private static readonly int Start = Animator.StringToHash("Start");
        private static readonly int End = Animator.StringToHash("End");

        public static event Action OnTransitionedToNewScene;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (_, _) => PhotonNetwork.IsMessageQueueRunning = true;
        }

        private IEnumerator Transition(string scene, bool useTransition)
        {
            animator.ResetTrigger(Start);
            animator.ResetTrigger(End);

            SoundManager.Play(GameConstants.Sounds.SceneTransition);
            if (useTransition)
            {
                animator.SetTrigger(Start);
                yield return new WaitForSeconds(transitionTime);
            }

            // PhotonNetwork.LoadLevel(scene);
            
            // yield return new WaitUntil(() =>
            // {
                // var levelLoadingProgress = PhotonNetwork.LevelLoadingProgress;
                
                // if (levelLoadingProgress == 0 && PhotonNetwork.AsyncLevelLoadingOperation == null)
                // {
                    // var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                    // if (activeScene.isLoaded && activeScene.name == scene)
                    // {
                        // return true;
                    // }
                // }
                
                // return levelLoadingProgress >= 1;
            // });

            PhotonNetwork.IsMessageQueueRunning = false;
            var asyncLoadScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
            yield return new WaitUntil(() => asyncLoadScene.isDone);
            
            OnTransitionedToNewScene?.Invoke();
            SoundManager.Play(GameConstants.Sounds.SceneTransitionFinish);

            if (useTransition)
            {
                animator.SetTrigger(End);
            }
        }

        public static void TransitionToScene(string scene, bool useTransition = true)
        {
            _instance.StartCoroutine(_instance.Transition(scene, useTransition));
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Audio;

namespace Utils
{
    public class SceneTransition : MonoBehaviour
    {
        private static SceneTransition _instance;

        [SerializeField] private float transitionTime = 1;
        [SerializeField] private Animator animator;
        
        private static readonly int Start = Animator.StringToHash("Start");
        private static readonly int End = Animator.StringToHash("End");

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
        
            Destroy(gameObject);
        }

        private IEnumerator Transition(int scene, bool useTransition)
        {
            animator.ResetTrigger(Start);
            animator.ResetTrigger(End);
            
            SoundManager.Play(GameConstants.Sounds.SceneTransition);
            if (useTransition)
            {
                animator.SetTrigger(Start);
                yield return new WaitForSeconds(transitionTime);
            }

            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            
            SoundManager.Play(GameConstants.Sounds.SceneTransitionFinish);
            if (useTransition)
            {
                animator.SetTrigger(End);
            }
        }

        public static void TransitionToScene(GameConstants.SceneIndices scene, bool useTransition = true)
        {
            _instance.StartCoroutine(_instance.Transition((int) scene, useTransition));
        }
    }
}
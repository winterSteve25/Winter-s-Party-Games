using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneTransition : MonoBehaviour
    {
        private static SceneTransition _instance;

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

        private IEnumerator Transition(int scene)
        {
            // TODO
            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        }

        public static void TransitionToScene(GameConstants.SceneIndices scene)
        {
            _instance.StartCoroutine(_instance.Transition((int) scene));
        }
    }
}
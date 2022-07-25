using UnityEngine;

namespace Utils
{
    public class SnowLight : MonoBehaviour
    {
        private static SnowLight _instance;

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
    }
}
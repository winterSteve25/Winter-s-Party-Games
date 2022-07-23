using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Games.Utils
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private Slider progressBar;
        
        public int timeLimit;
        public UnityEvent onComplete;
        
        private float _internalCounter;
        private bool _started;
        
        public void StartTimer()
        {
            _internalCounter = timeLimit;
            progressBar.maxValue = _internalCounter;
            _started = true;
        }

        private void Update()
        {
            if (!_started) return;
            
            _internalCounter -= Time.deltaTime;

            if (_internalCounter <= 0)
            {
                _started = false;
                onComplete.Invoke();
                return;
            }
            
            progressBar.value = _internalCounter;
        }
    }
}
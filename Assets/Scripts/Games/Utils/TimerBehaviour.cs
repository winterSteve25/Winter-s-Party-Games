using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Games.Utils
{
    public class TimerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Slider progressBar;
        
        public int timeLimit;
        
        public UnityEvent onComplete;
        public bool IsComplete { get; private set; }

        private Timer _timer = new();

        public void StartTimer()
        {
            _timer.StartTimer(timeLimit);
            progressBar.maxValue = timeLimit;
        }

        private void Update()
        {
            _timer.Tick();
            progressBar.value = _timer.Counter;
        }
    }
}
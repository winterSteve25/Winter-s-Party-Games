using UnityEngine;

namespace Games.Utils
{
    public class Timer
    {
        public bool IsComplete { get; private set; }
        public float Counter { get; private set; }
        
        private int _timeLimit;
        private bool _started;
        
        public void StartTimer(int timeLimit)
        {
            _timeLimit = timeLimit;
            Counter = timeLimit;
            _started = true;
        }

        public void Tick()
        {
            if (!_started) return;
            
            Counter -= Time.deltaTime;

            if (Counter <= 0)
            {
                _started = false;
                IsComplete = true;
            }
        }
    }
}
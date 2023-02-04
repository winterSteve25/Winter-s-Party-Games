using UnityEngine;

namespace Games.Utils
{
    public class Timer
    {
        public bool IsComplete { get; private set; }
        public float Counter;
        
        private int _timeLimit;
        private bool _started;
        private bool _reversed;
        
        public void StartTimer(int timeLimit)
        {
            _timeLimit = timeLimit;
            Counter = timeLimit;
            _reversed = false;
            _started = true;
            IsComplete = false;
        }

        public void StartReverseTimer(int timeLimit)
        {
            _timeLimit = timeLimit;
            Counter = 0;
            _reversed = true;
            _started = true;
            IsComplete = false;
        }

        public void Tick()
        {
            if (!_started) return;

            if (_reversed)
            {
                Counter += Time.deltaTime;

                if (Counter >= _timeLimit)
                {
                    _started = false;
                    IsComplete = true;
                }
            }
            else
            {
                Counter -= Time.deltaTime;

                if (Counter <= 0)
                {
                    _started = false;
                    IsComplete = true;
                }
            }
        }
    }
}
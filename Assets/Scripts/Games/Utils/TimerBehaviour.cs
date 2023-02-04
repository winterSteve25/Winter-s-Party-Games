using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Games.Utils
{
    public class TimerBehaviour : MonoBehaviour, IPunObservable
    {
        [SerializeField]
        private Slider progressBar;
        
        public int timeLimit;
        
        public UnityEvent onComplete;
        public bool IsComplete { get; private set; }

        private Timer _timer = new();
        private bool _wasComplete;

        public void StartTimer()
        {
            _timer.StartTimer(timeLimit);
            progressBar.maxValue = timeLimit;
        }

        private void Update()
        {
            _timer.Tick();
            progressBar.value = _timer.Counter;
            IsComplete = _timer.IsComplete;

            if (!_wasComplete && IsComplete)
            {
                onComplete.Invoke();
            }
            
            _wasComplete = IsComplete;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            stream.Serialize(ref _timer.Counter);
        }
    }
}
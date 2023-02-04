using System.Collections.Generic;
using Games.Base.Submissions;
using Games.Utils;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Games.Base
{
    public abstract class GameStageManager : MonoBehaviour
    {
        [SerializeField, Required] protected TimerBehaviour timerBehaviour;
        [SerializeField, Required] protected SubmissionIndicator submissionIndicator;

        [SerializeField] protected List<GameObject> gamePlayObjects;
        [SerializeField] protected List<GameObject> finishedObjects;

        protected virtual void Start()
        {
            timerBehaviour.StartTimer();
            foreach (var go in finishedObjects)
            {
                go.SetActive(false);
            }
        }
        
        public virtual void Submit()
        {
            submissionIndicator.Submit();
            OnSubmit(PhotonNetwork.LocalPlayer.ActorNumber);
            
            foreach (var go in gamePlayObjects)
            {
                go.SetActive(false);
            }

            foreach (var go in finishedObjects)
            {
                go.SetActive(true);
            }
        }

        protected abstract void OnSubmit(int localPlayerActorNumber);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Network.Sync;
using Photon.Pun;
using Photon.Realtime;

namespace Games.Base.Submissions
{
    public class SubmissionManager<T>
    {
        protected static readonly Serializer SubmissionSerializer = stringSubmission => $"{stringSubmission.SubmitterActorID}|{stringSubmission.SubmissionContent}";

        public delegate object Serializer(Submission<T> value);
        public delegate Submission<T> Deserializer(object value);
        
        private SerializedSyncedList<Submission<T>> _submissionsSyncedList;
        private Action _onComplete;
        private bool _finished;

        public IReadOnlyList<Submission<T>> Submissions => _submissionsSyncedList.Value.AsReadOnly();

        public SubmissionManager(Deserializer deserializer, Action onComplete) : this(SubmissionSerializer, deserializer, onComplete)
        {
        }

        public SubmissionManager(Serializer serializer, Deserializer deserializer, Action onComplete)
        {
            _submissionsSyncedList = new SerializedSyncedList<Submission<T>>(
                    sub => serializer(sub), 
                    data => deserializer(data), 
                    onChanged: SubmissionChanged
            );

            _onComplete = onComplete;
        }
        
        public void AddSubmission(Submission<T> submission)
        {
            if (_finished) return;
            _submissionsSyncedList.Add(submission);
        }

        private void SubmissionChanged()
        {
            if (_finished) return;
            EnoughSubmissions();
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_finished) return;
            var sub = _submissionsSyncedList.FirstOrDefault(s => s.SubmitterActorID == otherPlayer.ActorNumber);

            if (sub != null)
            {
                _submissionsSyncedList.Remove(sub);
            }
            
            EnoughSubmissions();
        }

        private void EnoughSubmissions()
        {
            if (!LobbyData.Instance.IsHost) return;
            if (_submissionsSyncedList.Count >= PhotonNetwork.PlayerList.Length)
            {
                _finished = true;
                _onComplete();
            }
        }
    }
}
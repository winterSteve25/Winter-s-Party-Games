using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FreeDraw;
using Games.Base;
using Games.Base.Submissions;
using Games.ScrambledEggs.Data;
using Games.Utils;
using Photon.Pun;
using Settings;
using TMPro;
using UnityEngine;
using Utils;

namespace Games.ScrambledEggs.Procedure
{
    public class ScrambledEggsDrawingTask : MonoBehaviour
    {
        [SerializeField] private TimerBehaviour timer;
        [SerializeField] private ScrambledEggsOfDoomSettings settings;
        [SerializeField] private Drawable drawing;
        [SerializeField] private TextMeshProUGUI sentence;
        [SerializeField] private SubmissionIndicator submissionIndicator;

        [SerializeField] private GameObject[] inputs;
        [SerializeField] private GameObject waitingMessage;

        [SerializeField] private GameConstants.SceneIndices transitionTo;
        [SerializeField] private int submitToIndex;
        
        private bool _submitted;

        private void Awake()
        {
            timer.timeLimit = settings.timeLimitDrawing;
            timer.StartTimer();
        }

        private void Start()
        {
            waitingMessage.SetActive(false);
        }

        private void OnEnable()
        {
            timer.onComplete.AddListener(TimerComplete);
        }

        private void OnDisable()
        {
            timer.onComplete.RemoveListener(TimerComplete);
        }

        public void Submit()
        {
            if (_submitted) return;

            foreach (var go in inputs)
            {
                go.SetActive(false);
            }

            waitingMessage.SetActive(true);

            var texture = drawing.drawable_texture;
            PhotonView.Get(this).RPC(nameof(SubmitRPC), RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber,
                texture.width, texture.height, DataUtilities.SerializeTexture2D(texture), sentence.text);
            _submitted = true;
        }

        private void TimerComplete()
        {
            Submit();
            StartCoroutine(Next());
        }

        private Dictionary<int, List<int>> _receiveStatus = new();
        private bool _receivedAll;

        [PunRPC]
        private void SubmitRPC(int actorID, int width, int height, byte[] content, string context)
        {
            var texture = new Texture2D(width, height);
            texture.SetPixels(DataUtilities.DeserializeTexture2DPixels(content));
            texture.Apply();

            GlobalData.Read<ScrambledEggsGameData>(GameConstants.GlobalData.LatestGameData)
                .GetDrawingTask(submitToIndex)
                .Add(new ScrambledEggsPaintingSubmission(actorID, texture, context));

            PhotonView.Get(this).RPC(nameof(DataReceived), RpcTarget.AllBufferedViaServer, actorID,
                PhotonNetwork.LocalPlayer.ActorNumber);
        }

        // this is necessary because images take a while to send through the network. and the submitter always receives the data first without insurance that others have received the information as well, therefore creating a de-sync
        [PunRPC]
        private void DataReceived(int receivedSubmissionFrom, int whoReceived)
        {
            if (!_receiveStatus.ContainsKey(whoReceived))
            {
                _receiveStatus.Add(whoReceived, new List<int>());
            }

            // make note that this player (whoReceived) has received submission from another player (receivedSubmissionFrom)
            _receiveStatus[whoReceived].Add(receivedSubmissionFrom);

            submissionIndicator.Submit(receivedSubmissionFrom).OnComplete(() =>
            {
                // if all players has received some submission
                var allPlayersHaveReceivedSubmissions = PhotonNetwork.CurrentRoom.Players.Keys.All(p => _receiveStatus.ContainsKey(p));
                var allPlayersHaveReceivedAllSubmissions = _receiveStatus.Keys.All(receivers =>
                {
                    var allReceivedSubmissions = _receiveStatus[receivers];
                    return PhotonNetwork.CurrentRoom.Players.Keys.All(p => allReceivedSubmissions.Contains(p));
                });

                _receivedAll = allPlayersHaveReceivedSubmissions && allPlayersHaveReceivedAllSubmissions;
                
                if (_receivedAll)
                {
                    StartCoroutine(Next());
                }
            });
        }

        private IEnumerator Next()
        {
            yield return new WaitUntil(() => _receivedAll);
            SceneManager.TransitionToScene(transitionTo);
        }
    }
}
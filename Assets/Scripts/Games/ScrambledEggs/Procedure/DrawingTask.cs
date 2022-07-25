using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using FreeDraw;
using Games.Base;
using Games.ScrambledEggs.Data;
using Games.ScrambledEggs.Helpers;
using Games.Utils;
using Photon.Pun;
using Settings;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Dictionary;

namespace Games.ScrambledEggs.Procedure
{
    public class DrawingTask : MonoBehaviour
    {
        [SerializeField] private Timer timer;
        [SerializeField] private ScrambledEggsOfDoomSettings settings;
        [SerializeField] private Drawable drawing;
        [SerializeField] private TextMeshProUGUI sentence;
        [SerializeField] private SubmissionIndicator submissionIndicator;

        [SerializeField] private GameObject[] inputs;
        [SerializeField] private GameObject waitingMessage;
        
        private bool _submitted;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            
            var localPlayer = PhotonNetwork.LocalPlayer;
            var data = GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData);
            var stage4Submissions = data.GetSimpleTasks(4);
            var stage3Submissions = data.GetSimpleTasks(3);
            var stage2Submissions = data.GetSimpleTasks(2);

            sentence.text = $"{SubmissionHelper.FindSubmission(stage2Submissions, localPlayer, WordDictionary.GetRandomAdjective()).SubmissionContent} " +
                            $"{SubmissionHelper.FindSubmission(stage3Submissions, localPlayer, WordDictionary.GetRandomNoun()).SubmissionContent} of " +
                            $"{SubmissionHelper.FindSubmission(stage4Submissions, localPlayer, WordDictionary.GetRandomNoun()).SubmissionContent}";
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
            PhotonView.Get(this).RPC(nameof(SubmitRPC), RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, texture.width, texture.height, texture.GetPixels(), sentence.text);
            _submitted = true;
        }

        private void TimerComplete()
        {
            Submit();
            StartCoroutine(Next());
        }

        private int _dataReceived;
        
        [PunRPC]
        private void SubmitRPC(int actorID, int width, int height, Color[] content, string context)
        {
            var texture = new Texture2D(width, height);
            texture.SetPixels(content);
            texture.Apply();
            
            GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData).Stage5Submissions
                .Add(new PaintingSubmission(actorID, texture, context));

            _dataReceived++;
            
            submissionIndicator.Submit(actorID).OnComplete(() =>
            {
                // if all players has submitted stop waiting for the timer
                if (_dataReceived == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    TimerComplete();
                }
            });
        }

        private IEnumerator Next()
        {
            yield return new WaitUntil(() => _dataReceived >= PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.ScrambledEggsOfDoomVoting);
        }
    }
}
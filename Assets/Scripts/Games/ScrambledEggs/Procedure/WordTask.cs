using System;
using System.Collections;
using System.Globalization;
using Base;
using DG.Tweening;
using Games.Base;
using Games.ScrambledEggs.Data;
using Games.ScrambledEggs.Helpers;
using Games.Utils;
using Photon.Pun;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Dictionary;

namespace Games.ScrambledEggs.Procedure
{
    public class WordTask : MonoBehaviour
    {   
        [SerializeField] private int stage;
        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI word;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ScrambledEggsOfDoomSettings settings;

        [SerializeField] private GameObject inputs;
        [SerializeField] private GameObject waitingMessage;

        [SerializeField] private SubmissionIndicator submissionIndicator;
        
        private bool _submitted;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            var localPlayer = PhotonNetwork.LocalPlayer;
            
            switch (stage)
            {
                case 1:
                    GlobalData.Set(GameConstants.GlobalData.ScrambledEggsGameData, new GameData());
                    word.text = WordDictionary.GetRandomAdjective();
                    break;
                case >= 2 and < 4:
                    // sort all the submissions from the last stage by actor ids
                    var botSubmission = stage - 1 == 2 ? WordDictionary.GetRandomAdjective() : WordDictionary.GetRandomNoun();
                    var data = GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData).GetSimpleTasks(stage - 1);
                    word.text = SubmissionHelper.FindSubmission(data, localPlayer, botSubmission).SubmissionContent;
                    break;
                case 4:
                    // sort all the submissions from the last 2 stages by actor ids
                    var stage3Submissions = GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData).GetSimpleTasks(3);
                    var stage2Submissions = GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData).GetSimpleTasks(2);
                    word.text = SubmissionHelper.FindSubmission(stage2Submissions, localPlayer, WordDictionary.GetRandomAdjective()).SubmissionContent + " " + SubmissionHelper.FindSubmission(stage3Submissions, localPlayer, WordDictionary.GetRandomNoun()).SubmissionContent + " of ...";
                    break;
                default:
                    word.text = word.text;
                    break;
            }

            timer.timeLimit = settings.timeLimitWord;
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

        public void SubmitWord()
        {
            if (_submitted) return;
            
            inputs.SetActive(false);
            waitingMessage.SetActive(true);
            
            // if input is empty and it is stage 1, 3 or 4 we get a random noun, if its stage 2 get random adjective
            // if input is not null we make it Title Case
            var input = string.IsNullOrEmpty(inputField.text) ? stage == 2 ? WordDictionary.GetRandomAdjective() : WordDictionary.GetRandomNoun() : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(inputField.text);
            // submit input to other all players
            PhotonView.Get(this).RPC(nameof(SubmitRPC), RpcTarget.All, stage, PhotonNetwork.LocalPlayer.ActorNumber, input.Replace("\r", ""));
            _submitted = true;
        }

        private void TimerComplete()
        {
            SubmitWord();
            StartCoroutine(Next());
        }

        private int _dataReceived;
        
        [PunRPC]
        private void SubmitRPC(int stage1, int actorID, string content)
        {
            // add submission
            GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData)
                .GetSimpleTasks(stage1)
                .Add(new Submission<string>(actorID, content));

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

            switch (stage)
            {
                case 1:
                    SceneTransition.TransitionToScene(GameConstants.SceneIndices.ScrambledEggsOfDoomStage2);
                    break;
                case 2:
                    SceneTransition.TransitionToScene(GameConstants.SceneIndices.ScrambledEggsOfDoomStage3);
                    break;
                case 3:
                    SceneTransition.TransitionToScene(GameConstants.SceneIndices.ScrambledEggsOfDoomStage4);
                    break;
                case 4:
                    SceneTransition.TransitionToScene(GameConstants.SceneIndices.ScrambledEggsOfDoomStage5);
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Base;
using Games.Base.Submissions;
using Games.GameModes.WordMania.PromptManagement;
using Games.GameModes.WordMania.VoteManagement;
using KevinCastejon.MoreAttributes;
using Network.Sync;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Utils;

namespace Games.GameModes.WordMania
{
    public class WordManiaGameManager : MonoBehaviourPunCallbacks
    {
        public static WordManiaGameManager Instance => _instance;
        private static WordManiaGameManager _instance;

        public WordManiaGameState CurrentGameState
        {
            get => _currentGameState.Value;
            set => _currentGameState.Value = value;
        }
        public SubmissionManager<string> StorySubmissions { get; private set; }
        public SubmissionManager<WordManiaPromptSubmission> AnswerSubmissions { get; private set; }

        public SyncedList<string> GamePrompts { get; private set; }
        public SyncedList<int> ChosenPrompts { get; private set; }
        public List<int> MyPrompts { get; private set; }

        private SyncedVar<WordManiaGameState> _currentGameState;
        
        [Scene, SerializeField] private string answerScene;
        [Scene, SerializeField] private string voteScene;
        [Scene, SerializeField] private string winnerScene;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            _currentGameState = new SyncedVar<WordManiaGameState>(WordManiaGameState.Story, onChanged: OnStateChanged);
            StorySubmissions = new StringSubmissionManager(() => CurrentGameState = WordManiaGameState.Answer);
            AnswerSubmissions = new WordManiaPromptSubmissionManager(() => CurrentGameState = WordManiaGameState.Vote);

            GamePrompts = new SyncedList<string>();
            ChosenPrompts = new SyncedList<int>();
            MyPrompts = new List<int>();
        }

        private void OnStateChanged()
        {
            switch (CurrentGameState)
            {
                case WordManiaGameState.Story:
                    break;
                case WordManiaGameState.Answer:
                    SceneManager.TransitionToScene(answerScene);
                    break;
                case WordManiaGameState.Vote:
                    SceneManager.TransitionToScene(voteScene);
                    break;
                case WordManiaGameState.Winner:
                    SceneManager.TransitionToScene(winnerScene);
                    break;
                case WordManiaGameState.Finished:
                    SceneManager.TransitionToScene(LobbyData.Instance.PartyGame.roomScene);
                    Destroy(LobbyData.Instance.gameObject);
                    Destroy(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            StorySubmissions.OnPlayerLeftRoom(otherPlayer);
            AnswerSubmissions.OnPlayerLeftRoom(otherPlayer);
        }
    }
}
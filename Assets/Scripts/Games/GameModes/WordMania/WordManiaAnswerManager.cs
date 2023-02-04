using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Base;
using Games.Base.Submissions;
using Games.GameModes.WordMania.PromptManagement;
using Games.Utils;
using Network.Sync;
using Photon.Pun;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Dictionary;
using Random = UnityEngine.Random;

namespace Games.GameModes.WordMania
{
    public class WordManiaAnswerManager : MonoBehaviour
    {
        public const int WordChoiceLimit = 10;
        public static readonly Regex WordsRegex = new("\\w+");

        private SyncedList<string> _prompts;
        private List<string> _myPrompts;
        private List<string> _myWords;
        private List<string> _words;

        [SerializeField, Required] private WordManiaPromptManager promptManager;
        [SerializeField, Required] private TextMeshProUGUI finishedText;
        [SerializeField, Required] private GameObject answerArea;
        [SerializeField, Required] protected TimerBehaviour timerBehaviour;
        [SerializeField, Required] protected SubmissionIndicator submissionIndicator;

        private string _firstAnswer = string.Empty;
        private string _secondAnswer = string.Empty;

        private void Awake()
        {
            _myPrompts = new List<string>();
            _myWords = new List<string>();
            _words = WordManiaGameManager.Instance.StorySubmissions.Submissions
                .SelectMany(story => WordsRegex
                    .Matches(story.SubmissionContent)
                    .Select(match => match.Value)
                )
                .Distinct()
                .ToList();
        }

        private void Start()
        {
            _prompts = WordManiaGameManager.Instance.GamePrompts;
            _prompts.OnChanged = InitializePrompt;

            if (LobbyData.Instance.IsHost)
            {
                _prompts.AddAll(EnumerableUtils.Collect(RandomPrompt, PhotonNetwork.PlayerList.Length));
            }

            timerBehaviour.StartTimer();
        }

        public void Submit()
        {
            if (timerBehaviour.IsComplete)
            {
                _firstAnswer = promptManager.GetResult();
                _secondAnswer = "";
                FinishSubmit();
                return;
            }

            if (string.IsNullOrEmpty(_firstAnswer))
            {
                _firstAnswer = promptManager.GetResult();
                RandomizeWords();
                promptManager.SetPrompt(ProcessPrompt(_myPrompts[1]), _myWords);
            }
            else
            {
                _secondAnswer = promptManager.GetResult();
                FinishSubmit();
            }

            void FinishSubmit()
            {
                answerArea.SetActive(false);
                submissionIndicator.Submit();
                finishedText.gameObject.SetActive(true);
                WordManiaGameManager.Instance.AnswerSubmissions.AddSubmission(new Submission<WordManiaPromptSubmission>(
                    new WordManiaPromptSubmission(
                        WordManiaGameManager.Instance.MyPrompts[0],
                        WordManiaGameManager.Instance.MyPrompts[1],
                        _firstAnswer,
                        _secondAnswer
                    )));
            }
        }

        private void RandomizeWords()
        {
            _myWords.Clear();

            var limit = Mathf.Min(_words.Count, WordChoiceLimit);
            var randomIndices = new HashSet<int>(limit);

            while (randomIndices.Count < limit)
            {
                randomIndices.Add(Random.Range(0, _words.Count));
            }

            foreach (var index in randomIndices)
            {
                _myWords.Add(_words[index]);
            }
        }

        private void InitializePrompt()
        {
            _myPrompts.Clear();

            for (var i = 0; i <= 1; i++)
            {
                var index = (PhotonNetwork.LocalPlayer.ActorNumber + i) % _prompts.Count;
                _myPrompts.Add(_prompts[index]);
                WordManiaGameManager.Instance.MyPrompts.Add(index);
                WordManiaGameManager.Instance.ChosenPrompts.Add(index);
            }

            RandomizeWords();
            promptManager.SetPrompt(ProcessPrompt(_myPrompts[0]), _myWords);
        }

        private static string RandomPrompt()
        {
            return WordDictionary.GetLineInFile("WordMania_Prompts");
        }

        private static string ProcessPrompt(string original)
        {
            var playerList = PhotonNetwork.PlayerList;
            return original.Replace("{player}", playerList[Random.Range(0, playerList.Length)].NickName);
        }
    }
}
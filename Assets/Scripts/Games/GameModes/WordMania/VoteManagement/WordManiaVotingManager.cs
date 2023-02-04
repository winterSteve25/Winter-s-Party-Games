using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using DG.Tweening;
using Games.Base.Submissions;
using Games.GameModes.WordMania.PromptManagement;
using Games.Utils;
using Network.Sync;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Games.GameModes.WordMania.VoteManagement
{
    public class WordManiaVotingManager : MonoBehaviour
    {
        private List<Submission<WordManiaPromptSubmission>>_submissions;
        private List<int> _prompts;
        private List<int> _myPrompts;
        private SyncedVar<int> _promptOn;
        private int _promptOnIndexInPromptsList;
        private bool _initialized;
        private Player[] _playersInRoom;

        private SerializedSyncedList<WordManiaVote> _votes;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private WordManiaVoteOption leftChoice;
        [SerializeField] private WordManiaVoteOption rightChoice;
        [SerializeField] private TimerBehaviour timerBehaviour;
        
        private void Awake()
        {
            _promptOn = new SyncedVar<int>(onChanged: () =>
            {
                StopAllCoroutines();
                StartCoroutine(RefreshDisplay());
                
                if (!_initialized)
                {
                    _promptOnIndexInPromptsList = 0;
                    return;
                }

                _promptOnIndexInPromptsList = _prompts.IndexOf(_promptOn.Value);
            });
            
            _votes = new SerializedSyncedList<WordManiaVote>(WordManiaVote.Serialize, WordManiaVote.Deserialize, onChanged: HasAllVotes);
        }

        private void Start()
        {
            _submissions = new List<Submission<WordManiaPromptSubmission>>();
            _prompts = new List<int>();
            _myPrompts = new List<int>();
            _playersInRoom = PhotonNetwork.PlayerList;
            
            foreach (var submission in WordManiaGameManager.Instance.AnswerSubmissions.Submissions)
            {
                _submissions.Add(submission);
            }

            foreach (var chosenPromptIndex in WordManiaGameManager.Instance.ChosenPrompts
                         .Distinct()
                         .OrderBy(a => a))
            {
                _prompts.Add(chosenPromptIndex);
            }

            foreach (var myPromptIndex in WordManiaGameManager.Instance.MyPrompts)
            {
                _myPrompts.Add(myPromptIndex);
            }

            _initialized = true;
            
            if (LobbyData.Instance.IsHost)
            {
                _promptOn.Value = _prompts.First();
            }
        }

        private IEnumerator RefreshDisplay()
        {
            // make sure we have the necessary data before refreshing display
            if (!_initialized)
            {
                yield return new WaitUntil(() => _initialized);
            }
         
            var promptIndex = _promptOn.Value;
            title.text = WordManiaGameManager.Instance.GamePrompts[promptIndex];
            
            var answers = _submissions.FindAll(sub => sub.SubmissionContent.Prompt1 == promptIndex || sub.SubmissionContent.Prompt2 == promptIndex);
            if (answers.Count < 2)
            {
                Debug.LogError("Less than 2 submissions found on a single prompt");
                yield break;
            }

            if (answers.Count > 2)
            {
                Debug.LogWarning("More than 2 submissions found on a single prompt");
            }

            var firstSubmission = answers[0].SubmissionContent;
            leftChoice.Text = firstSubmission.Prompt1 == promptIndex ? firstSubmission.FirstAnswer : firstSubmission.SecondAnswer;
            
            var secondSubmission = answers[1].SubmissionContent;
            rightChoice.Text = secondSubmission.Prompt1 == promptIndex ? secondSubmission.FirstAnswer : secondSubmission.SecondAnswer;

            // var iSubmitted = _myPrompts.Contains(promptIndex);
            // leftChoice.Interactable = !iSubmitted;
            // rightChoice.Interactable = !iSubmitted;
        }

        public void Vote(bool left)
        {
            _votes.Add(new WordManiaVote(
                    PhotonNetwork.LocalPlayer.ActorNumber,
                    _promptOn.Value,
                    left ? 0 : 1
            ));

            var chose = left ? leftChoice : rightChoice;
            var other = left ? rightChoice : leftChoice;
            
            chose.transform.DOScale(1.25f, 0.2f)
                .SetEase(Ease.OutBounce);

            other.transform.DOScale(0.8f, 0.2f)
                .SetEase(Ease.InBounce);
            
            chose.Interactable = false;
            other.Interactable = false;
        }

        public void HasAllVotes()
        {
            var voteCountOnPrompt = _votes.Count(vote => vote.VoterVotedPromptIndex == _promptOn.Value);
            if (timerBehaviour.IsComplete || voteCountOnPrompt > _playersInRoom.Length - 2)
            {
                DisplayVoteResults();
            }
        }

        private void DisplayVoteResults()
        {
            if (!LobbyData.Instance.IsHost) return;

            var nextPromptIndex = _promptOnIndexInPromptsList + 1;
            if (nextPromptIndex >= _prompts.Count)
            {
                FinishedAllVotes();
                return;
            }
            
            _promptOn.Value = _prompts[nextPromptIndex];
        }

        private void FinishedAllVotes()
        {
            if (!LobbyData.Instance.IsHost) return;
            WordManiaGameManager.Instance.CurrentGameState = WordManiaGameState.Winner;
        }
    }
}
using System.Collections;
using System.Globalization;
using DG.Tweening;
using Games.Base;
using Games.Base.Submissions;
using Games.ScrambledEggs.Data;
using Games.Utils;
using Photon.Pun;
using Settings;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Audio;
using Utils.Dictionary;

namespace Games.ScrambledEggs.Procedure
{
    public class ScrambledEggsWordTask : MonoBehaviour
    {
        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI word;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ScrambledEggsOfDoomSettings settings;
        [SerializeField] private SubmissionIndicator submissionIndicator;

        [SerializeField] private GameObject inputs;
        [SerializeField] private GameObject waitingMessage;
        
        [SerializeField] private RectTransform emptyInputPrompt;
        [SerializeField] private CanvasGroup yesButton;
        [SerializeField] private CanvasGroup noButton;

        [SerializeField] private GameConstants.SceneIndices transitionTo;
        [SerializeField] private PartOfSpeech randomAnswerType;
        [SerializeField] private int submitToIndex;
        
        private bool _submitted;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            timer.timeLimit = settings.timeLimitWord;
            timer.StartTimer();
        }

        private void Start()
        {
            emptyInputPrompt.gameObject.SetActive(false);
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
            StartCoroutine(SubmitWordCoroutine());
        }

        private bool _answered;
        private bool _yes;
        
        private IEnumerator SubmitWordCoroutine()
        {
            // if input is empty and it is stage 1, 3 or 4 we get a random noun, if its stage 2 get random adjective
            // if input is not null we make it Title Case

            string input;

            if (string.IsNullOrEmpty(inputField.text) && !timer.IsComplete)
            {
                emptyInputPrompt.localScale = Vector3.zero;
                emptyInputPrompt.gameObject.SetActive(true);
                emptyInputPrompt.DOScale(Vector3.one, 0.2f);
                SoundManager.Play(GameConstants.Sounds.PopUp);
                yield return new WaitForSeconds(0.5f);
                yesButton.DOFade(1, 0.5f);
                noButton.DOFade(1, 0.5f);
                
                yield return new WaitUntil(() =>
                {
                    // if timer is complete default to yes and submit the answer
                    if (timer.IsComplete)
                    {
                        _yes = true;
                        return true;
                    }
                    return _answered;
                });

                SoundManager.Play(GameConstants.Sounds.SceneTransitionFinish);
                emptyInputPrompt.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                {
                    emptyInputPrompt.gameObject.SetActive(false);
                });

                if (_yes)
                {
                    input = WordDictionary.GetRandom(randomAnswerType);
                    input = input.Replace("\r", "");
                }
                else
                {
                    inputs.SetActive(true);
                    waitingMessage.SetActive(false);
                    _submitted = false;

                    _answered = false;
                    _yes = false;
                    
                    yield break;
                }
            }
            else
            {
                input = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(inputField.text);
            }
            
            inputs.SetActive(false);
            waitingMessage.SetActive(true);
            _submitted = true;
            
            // submit input to other all players
            PhotonView.Get(this).RPC(nameof(SubmitRPC), RpcTarget.All, submitToIndex, PhotonNetwork.LocalPlayer.ActorNumber, input);
        }

        public void YesToEmpty()
        {
            _yes = true;
            _answered = true;
        }

        public void NoToEmpty()
        {
            _yes = false;
            _answered = true;
        }
        
        private void TimerComplete()
        {
            SubmitWord();
            StartCoroutine(Next());
        }

        private int _dataReceived;
        
        [PunRPC]
        private void SubmitRPC(int index, int actorID, string content)
        {
            // add submission
            GlobalData.Read<ScrambledEggsGameData>(GameConstants.GlobalData.LatestGameData)
                .GetWordTask(index)
                .Add(new Submission<string>(actorID, content.Replace("\r", "")));

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
            SceneTransition.TransitionToScene(transitionTo);
        }
    }
}
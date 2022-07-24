using System.Collections;
using FreeDraw;
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

        private bool _submitted;

        private void Awake()
        {
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
            var texture = drawing.drawable_texture;
            PhotonView.Get(this).RPC(nameof(SubmitRPC), RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, texture.width, texture.height, texture.GetPixels());
            _submitted = true;
        }

        private void TimerComplete()
        {
            Submit();
            PhotonView.Get(this).RPC(nameof(NextSceneRPC), RpcTarget.All);
        }

        [PunRPC]
        private void SubmitRPC(int actorID, int width, int height, Color[] content)
        {
            var texture = new Texture2D(width, height);
            texture.SetPixels(content);
            texture.Apply();
            
            GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData).Stage5Submissions
                .Add(new Submission<Texture2D>(actorID, texture));
        }
        
        [PunRPC]
        private void NextSceneRPC()
        {
            StartCoroutine(Next());
        }

        private static IEnumerator Next()
        {
            yield return new WaitForSeconds(1f);
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.ScrambledEggsOfDoomVoting);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Games.Base.Scoreboard
{
    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private GameObject playerScorePrefab;
        [SerializeField] private Transform[] spots;
        
        private List<Score> _scoreboard;

        public GameConstants.SceneIndices transitionTo;

        private void Awake()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        private IEnumerator Start()
        {
            _scoreboard = new List<Score>();

            var players = LobbyData.Instance.Players;
            var scoring = GlobalData.Read<Dictionary<int, int>>(GameConstants.GlobalData.LatestScoring);
            var previousScoring = GlobalData.Read<Dictionary<int, int>>(GameConstants.GlobalData.PreviousScoring);
            
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var id = player.actorID;
                
                var go = Instantiate(playerScorePrefab, transform);
                go.transform.position = spots[i].position;

                var score = previousScoring[id];
                var scoreboardItem = go.GetComponent<ScoreboardItem>();
                
                scoreboardItem.SetAmount(score);
                scoreboardItem.player.data = player;
                scoreboardItem.player.UpdateAppearance();
                
                _scoreboard.Add(new Score(score, id, scoreboardItem));
            }
            
            UpdateScoreboard(previousScoring);
            yield return new WaitForSeconds(2f);
            UpdateScoreboard(scoring);
            yield return new WaitForSeconds(5f);
            SceneTransition.TransitionToScene(transitionTo);
        }

        private void UpdateScoreboard(IReadOnlyDictionary<int, int> scoring)
        {
            foreach (var kv in _scoreboard)
            {
                var p = PhotonNetwork.CurrentRoom.GetPlayer(kv.actorID);
                var newScore = scoring[p.ActorNumber];
                kv.score = newScore;
            }
            
            var sorted = (from entry in _scoreboard orderby entry.score descending select entry).ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                var entry = sorted[i];
                var spot = spots[i];
                var scoreboardItem = entry.item;
                scoreboardItem.SetScore(entry.score, () =>
                {
                    var scoreboardItemTransform = scoreboardItem.transform;
                    var spotTransform = spot.transform;
                    
                    if (scoreboardItemTransform.position != spotTransform.position)
                    {
                        scoreboardItemTransform.DOMove(spotTransform.position, 2f).SetEase(Ease.OutCubic);
                    }
                });
            }
        }
    }
}
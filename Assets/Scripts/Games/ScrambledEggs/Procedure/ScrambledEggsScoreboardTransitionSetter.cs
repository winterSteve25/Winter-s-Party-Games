using Games.Base.Scoreboard;
using Games.ScrambledEggs.Data;
using UnityEngine;
using Utils;

namespace Games.ScrambledEggs.Procedure
{
    public class ScrambledEggsScoreboardTransitionSetter : MonoBehaviour
    {
        private void Awake()
        {
            var data = GlobalData.Read<ScrambledEggsGameData>(GameConstants.GlobalData.LatestGameData);
            var scoreboard = FindObjectOfType<Scoreboard>();

            scoreboard.transitionTo = data.GetRoundOn() switch
            {
                1 => GameConstants.SceneIndices.ScrambledEggsOfDoomR2S4,
                2 => GameConstants.SceneIndices.ScrambledEggsOfDoomR3S8,
                3 => GameConstants.SceneIndices.ScrambledEggsOfDoomWinner,
                _ => scoreboard.transitionTo
            };
        }
    }
}
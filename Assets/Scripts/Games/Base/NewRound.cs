using TMPro;
using UnityEngine;
using Utils;

namespace Games.Base
{
    public class NewRound : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roundText;

        private void Awake()
        {
            var gameData = GlobalData.Read<IGameData>(GameConstants.GlobalData.LatestGameData);
            gameData.NextRound();
            roundText.text = $"Round {gameData.GetRoundOn()}!";
        }
    }
}
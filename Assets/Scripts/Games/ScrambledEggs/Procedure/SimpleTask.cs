using ExitGames.Client.Photon;
using Games.ScrambledEggs.Data;
using Games.ScrambledEggs.Messages;
using Games.Utils;
using Network;
using Photon.Pun;
using Settings;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Dictionary;

namespace Games.ScrambledEggs.Procedure
{
    public class SimpleTask : MonoBehaviour
    {
        [SerializeField] private int stage;
        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI word;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ScrambledEggsOfDoomSettings settings;
        
        private void Awake()
        {
            word.text = stage switch
            {
                1 => WordDictionary.GetRandomAdjective(),
                // TODO
                _ => word.text
            };

            timer.timeLimit = settings.timeLimitSimple;
            timer.StartTimer();
            GlobalData.Set(GameConstants.GlobalData.ScrambledEggsGameData, new GameData());
        }

        private void OnEnable()
        {
            timer.onComplete.AddListener(SubmitWord);
        }

        private void OnDisable()
        {
            timer.onComplete.RemoveListener(SubmitWord);
        }

        public void SubmitWord()
        {
            PhotonView.Get(this).RPC(nameof(SubmitWordRPC), RpcTarget.All);
        }

        [PunRPC]
        private void SubmitWordRPC()
        {
            GlobalData.Read<GameData>(GameConstants.GlobalData.ScrambledEggsGameData)
                .GetSimpleTasks(stage)
                .Add(new Submission<string>(PhotonNetwork.LocalPlayer.ActorNumber, inputField.text));
        }
    }
}
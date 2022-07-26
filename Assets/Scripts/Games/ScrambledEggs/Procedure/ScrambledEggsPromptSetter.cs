using System;
using Games.Base;
using Games.ScrambledEggs.Data;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Dictionary;

namespace Games.ScrambledEggs.Procedure
{
    public class ScrambledEggsPromptSetter : MonoBehaviour
    {
        [SerializeField] private ScrambledEggsPromptPart[] promptStructure;
        [SerializeField] private TextMeshProUGUI promptText;

        private void Awake()
        {
            promptText.text = "";
            
            for (var i = 0; i < promptStructure.Length; i++)
            {
                var promptPart = promptStructure[i];
                
                switch (promptPart.type)
                {
                    case ScrambledEggsPromptPartTypes.ConstantWord:
                        promptText.text += promptPart.constantWord.Replace("\r", "");
                        break;
                    case ScrambledEggsPromptPartTypes.Submission:
                        var botSubmission = WordDictionary.GetRandom(promptPart.generateRandom);
                        var data = GlobalData.Read<ScrambledEggsGameData>(GameConstants.GlobalData.LatestGameData).GetWordTask(promptPart.submissionFromIndex);
                        var localPlayer = PhotonNetwork.LocalPlayer;
                        promptText.text += SubmissionHelper.FindSubmission(data, localPlayer, botSubmission).SubmissionContent.Replace("\r", "");
                        break;
                    case ScrambledEggsPromptPartTypes.RandomWord:
                        var rand = WordDictionary.GetRandom(promptPart.generateRandom);
                        promptText.text += rand.Replace("\r", "");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (i != promptStructure.Length - 1)
                {
                    promptText.text += " ";
                }
            }
        }
    }
}
using Games.Base;
using Games.Base.Submissions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Games.GameModes.WordMania
{
    public class WordManiaStoryManager : GameStageManager
    {
         [SerializeField, Required] private TMP_InputField story;
         
        protected override void OnSubmit(int localPlayerActorNumber)
        {
            WordManiaGameManager.Instance.StorySubmissions.AddSubmission(new StringSubmission(localPlayerActorNumber, story.text));
        }
    }
}
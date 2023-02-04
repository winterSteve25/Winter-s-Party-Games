using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Games.GameModes.WordMania.VoteManagement
{
    public class WordManiaVoteOption : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private WordManiaVotingManager votingManager;
        [SerializeField] private bool left;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Selectable selectable;
        
        public string Text
        {
            get => text.text;
            set => text.text = value;
        }

        public bool Interactable
        {
            get => selectable.IsInteractable();
            set => selectable.interactable = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!selectable.IsInteractable()) return;
            votingManager.Vote(left);
        }
    }
}
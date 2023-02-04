using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Games.GameModes.WordMania.PromptManagement
{
    public class WordManiaWordChoice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private TextMeshProUGUI text;
        private WordManiaPromptManager _promptManager;

        private void Start()
        {
            _promptManager = FindObjectOfType<WordManiaPromptManager>();
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _promptManager.DraggingWord = text.text;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _promptManager.DraggingWord = string.Empty;
        }
    }
}
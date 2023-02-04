using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Games.GameModes.WordMania.PromptManagement
{
    public class WordManiaPromptFillArea : MonoBehaviour, IDropHandler
    {
        private WordManiaPromptManager _promptManager;
        [SerializeField] private TextMeshProUGUI filledText;

        private void Start()
        {
            _promptManager = FindObjectOfType<WordManiaPromptManager>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(_promptManager.DraggingWord)) return;
            var text = Instantiate(filledText, transform);
            text.text = _promptManager.DraggingWord;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) transform.parent);
        }
    }
}
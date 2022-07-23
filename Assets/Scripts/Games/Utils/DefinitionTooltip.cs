using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Utils.Dictionary;

namespace Games.Utils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DefinitionTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup tooltip;
        [SerializeField] private TextMeshProUGUI text;

        private string _word;

        private void Start()
        {
            _word = GetComponent<TextMeshProUGUI>().text;
            this.FadeOutAndDeactivate(tooltip, 0f, null);
            StartCoroutine(WordDictionary.DefinitionOf(_word, definition => text.text += definition));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.FadeIn(tooltip, 0.2f, null);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.FadeOutAndDeactivate(tooltip, 0.2f, null);
        }
    }
}
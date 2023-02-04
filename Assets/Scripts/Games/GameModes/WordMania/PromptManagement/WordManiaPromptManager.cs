using System.Collections.Generic;
using System.Linq;
using System.Text;
using Games.Base.Submissions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GameModes.WordMania.PromptManagement
{
    public class WordManiaPromptManager : MonoBehaviour
    {
        [SerializeField, Required] private RectTransform promptArea;
        [SerializeField, Required] private RectTransform wordChoicesArea;

        [SerializeField, Required, AssetsOnly] private TextMeshProUGUI promptTextPrefab;
        [SerializeField, Required, AssetsOnly] private RectTransform filledAreaPrefab;
        [SerializeField, Required, AssetsOnly] private GameObject wordChoicePrefab;

        [SerializeField, Required] private TextMeshProUGUI draggingWordVisual;

        public string DraggingWord
        {
            get => _draggingWord;
            set
            {
                _draggingWord = value;
                if (string.IsNullOrEmpty(_draggingWord))
                {
                    draggingWordVisual.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    draggingWordVisual.transform.parent.gameObject.SetActive(true);
                    draggingWordVisual.text = _draggingWord;
                }
            }
        }
        private string _draggingWord;

        private void Start()
        {
            draggingWordVisual.transform.parent.gameObject.SetActive(false);
        }

        public string GetResult()
        {
            var stringBuilder = new StringBuilder();
            
            foreach (Transform child in promptArea)
            {
                if (!child.TryGetComponent<WordManiaPromptFillArea>(out _)) continue;
                var ts = child.GetComponentsInChildren<TextMeshProUGUI>();
                for (var index = 0; index < ts.Length; index++)
                {
                    var t = ts[index];
                    stringBuilder.Append(t.text);
                    if (index < ts.Length - 1)
                    {
                        stringBuilder.Append(" ");                            
                    }
                }

                stringBuilder.Append(",");
            }

            return stringBuilder.ToString()[..^1];
        }
        
        public void SetPrompt(string prompt, IEnumerable<string> choices)
        {
            Clear();
            
            var sections = prompt.Split("___");
            var sectionsCount = sections.Length;

            for (var i = 0; i < sectionsCount; i++)
            {
                var text = Instantiate(promptTextPrefab, promptArea);
                text.text = sections[i].Trim();
                
                if (i != sectionsCount - 1)
                {
                    Instantiate(filledAreaPrefab, promptArea);
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(promptArea);
            
            foreach (var choice in choices)
            {
                AddChoice(choice);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(wordChoicesArea);
        }

        private void AddChoice(string word)
        {
            var choice = Instantiate(wordChoicePrefab, wordChoicesArea);
            choice.GetComponentInChildren<TextMeshProUGUI>().text = word;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) choice.transform);
        }

        private void Clear()
        {
            foreach (Transform child in promptArea)
            {
                Destroy(child.gameObject);
            }
            
            foreach (Transform child in wordChoicesArea)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
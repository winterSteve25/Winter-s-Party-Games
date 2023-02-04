using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utils
{
    public class CharacterLimitCounter : MonoBehaviour
    {
        [SerializeField, Required] private TMP_InputField inputField;
        [SerializeField, Required] private TextMeshProUGUI text;

        private Color _normalTextColor;
        
        private void Start()
        {
            inputField.onValueChanged.AddListener(InputChanged);
            InputChanged(inputField.text);
            _normalTextColor = text.color;
        }

        private void InputChanged(string newValue)
        {
            var charCount = newValue.Length;
            var limit = inputField.characterLimit;
            
            text.text = $"{charCount}/{limit}";

            if (charCount >= limit)
            {
                text.color = Color.red;
            }
            else if (text.color == Color.red)
            {
                text.color = _normalTextColor;
            }
        }
    }
}
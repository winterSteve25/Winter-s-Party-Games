using Menu;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public class ReplaceUITheme : ScriptableWizard
    {
        private static TMP_FontAsset _fontAsset;
        private static ColorBlock _uiColorBlock;
        
        [MenuItem("Winter's Party Games/Replace UI Theme")]
        public static void CreateWizard()
        {
            _fontAsset = Resources.Load<TMP_FontAsset>("Fonts/Primary");
            _uiColorBlock = ColorBlock.defaultColorBlock;
            _uiColorBlock.normalColor = ConvertFrom255(135, 135, 135, 60);
            _uiColorBlock.disabledColor = ConvertFrom255(80, 80, 80, 60);
            _uiColorBlock.highlightedColor = ConvertFrom255(160, 160, 160, 60);
            _uiColorBlock.pressedColor = ConvertFrom255(100, 100, 100, 60);
            _uiColorBlock.selectedColor = ConvertFrom255(120, 120, 120, 60);
            
            DisplayWizard<ReplaceUITheme>("Replace UI Theme", "Replace");
        }

        public void OnWizardCreate()
        {
            var buttons = FindObjectsOfType<Button>();
            
            foreach (var btn in buttons)
            {
                btn.gameObject.AddComponent<SelectableAnimator>();

                var colorBlock = btn.colors;
                colorBlock.disabledColor = Color.clear;
                colorBlock.highlightedColor = Color.clear;
                colorBlock.normalColor = Color.clear;
                colorBlock.pressedColor = Color.clear;
                colorBlock.selectedColor = Color.clear;
                btn.colors = colorBlock;
                
                var tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
                tmp.fontSize = 28;
                tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
                tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
                
                Undo.RecordObject(btn, "Replace UI Theme");
            }

            var tmps = FindObjectsOfType<TMP_Text>();

            foreach (var tmp in tmps)
            {
                tmp.font = _fontAsset;
                tmp.color = Color.white;
                if (tmp.CompareTag("UITitle"))
                {
                    tmp.fontSize = 48;
                    tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
                    tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
                }

                Undo.RecordObject(tmp, "Replace UI Theme");
            }
            
            var dropDowns = FindObjectsOfType<TMP_Dropdown>();

            foreach (var dropDown in dropDowns)
            {
                dropDown.colors = _uiColorBlock;
                dropDown.GetComponent<Image>().sprite = null;
                
                var template = dropDown.template;
                var templateImage = template.GetComponent<Image>();
                templateImage.color = _uiColorBlock.normalColor;
                templateImage.sprite = null;

                var templateScrollRect = template.GetComponent<ScrollRect>();
                templateScrollRect.verticalScrollbar.colors = _uiColorBlock;
                templateScrollRect.verticalScrollbar.GetComponent<Image>().sprite = null;

                dropDown.itemText.font = _fontAsset;
                dropDown.itemText.transform.parent.gameObject.GetComponent<Toggle>().colors = _uiColorBlock;
                dropDown.itemText.color = Color.white;

                Undo.RecordObject(dropDown, "Replace UI Theme");
            }

            var inputFields = FindObjectsOfType<TMP_InputField>();

            foreach (var inputField in inputFields)
            {
                inputField.colors = _uiColorBlock;
                inputField.GetComponent<Image>().sprite = null;
            }

            var sliders = FindObjectsOfType<Slider>();

            foreach (var slider in sliders)
            {
                slider.colors = _uiColorBlock;
                
                var background = slider.transform.Find("Background");
                background.GetComponent<Image>().color = _uiColorBlock.pressedColor;
                
                var fill = slider.fillRect;
                fill.GetComponent<Image>().color = _uiColorBlock.highlightedColor;
            }
        }

        private static Color ConvertFrom255(float r, float g, float b, float a)
        {
            return new Color(r/255, g/255, b/255, a/255);
        }
    }
}
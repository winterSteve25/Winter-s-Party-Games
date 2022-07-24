using FreeDraw;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Utils.Paint
{
    public class ColorButton : MonoBehaviour
    {
        public Color color;
        
        private DrawingSettings _settings;

        private void Start()
        {
            _settings = FindObjectOfType<DrawingSettings>();
        }

        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Initialize(_settings);
        }

        public void Initialize(DrawingSettings settings)
        {
            settings.SetMarkerColour(color);
        }
    }
}
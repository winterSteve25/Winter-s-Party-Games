using FreeDraw;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Utils.Paint
{
    public abstract class PaintTool : MonoBehaviour
    {
        protected DrawingSettings Settings;

        private void Start()
        {
            Settings = FindObjectOfType<DrawingSettings>();
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
            Initialize(Settings);
        }

        public abstract void Initialize(DrawingSettings settings);
    }
}
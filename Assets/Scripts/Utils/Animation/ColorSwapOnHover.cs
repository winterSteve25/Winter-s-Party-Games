using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utils
{
    public class SwapColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField, Required] private Color normalColor;
        [SerializeField, Required] private Color hoverColor;
        [SerializeField, Required] private Color pressColor;

        [SerializeField, Required] private Image image;
        [SerializeField] private float lerpDuration = 0.5f;

        private bool _pointerOver;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerOver = true;
            image.DOColor(hoverColor, lerpDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerOver = false;
            image.DOColor(normalColor, lerpDuration);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            image.DOColor(pressColor, lerpDuration);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            image.DOColor(_pointerOver ? hoverColor : normalColor, lerpDuration);
        }
    }
}
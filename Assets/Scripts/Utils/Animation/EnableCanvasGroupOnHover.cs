using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.Animation
{
    public class EnableCanvasGroupOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration;
        
        private void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.DOFade(1, fadeDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            canvasGroup.DOFade(0, fadeDuration)
                .OnComplete(() => canvasGroup.gameObject.SetActive(false));
        }
    }
}
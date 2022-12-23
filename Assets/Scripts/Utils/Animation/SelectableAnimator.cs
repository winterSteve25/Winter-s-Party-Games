using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.Audio;
using Utils.Data;

namespace Utils.Animation
{
    [RequireComponent(typeof(Selectable))]
    [DisallowMultipleComponent]
    public class SelectableAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private bool doShake = true;

        private TextMeshProUGUI _text;
        private RectTransform _rectTransform;

        private Tween _shake;
        private bool _wasEnabled;
        private Selectable _button;

        private void Start()
        {
            _button = GetComponent<Selectable>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _rectTransform = GetComponent<RectTransform>();
            InteractableChanged(_button.interactable);
        }

        private void Update()
        {
            var newState = _button.IsInteractable();
            if (_wasEnabled != newState)
            {
                InteractableChanged(newState);
            }

            _wasEnabled = newState;
        }

        private void InteractableChanged(bool interactable)
        {
            if (_text is null) return;
            _text.color = interactable ? Color.white : Color.gray;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            SoundManager.Play(GameConstants.Sounds.SelectableHover);
            _rectTransform.DOScale(1.3f, 0.1f).SetEase(Ease.OutCubic);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            _rectTransform.DOScale(1f, 0.1f).SetEase(Ease.InCubic);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            _rectTransform.DOScale(1.5f, 0.1f).SetEase(Ease.OutCubic);
            if (_text is not null)
            {
                _text.color = Color.gray;
            }

            if (doShake)
            {
                _shake = _rectTransform.DOLocalRotate(new Vector3(0, 0, 3f), 0.1f).SetLoops(-1, LoopType.Yoyo);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_wasEnabled) return;
            if (Math.Abs(_rectTransform.localScale.x - 1) > 0.1f)
            {
                _rectTransform.DOScale(1.3f, 0.1f).SetEase(Ease.OutCubic);
            }

            _rectTransform.DOLocalRotate(Vector3.zero, 0.1f);
            if (_text is not null)
            {
                _text.color = Color.white;
            }

            if (doShake)
            {
                _shake.Kill();
                _shake = null;
            }
        }
    }
}
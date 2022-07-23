using System;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils
{
    public static class ObjectUtilities
    {
        public static void FadeOutAndDeactivate(this MonoBehaviour monoBehaviour, CanvasGroup canvasGroup, float duration, [CanBeNull] Action onFinished)
        {
            monoBehaviour.StartCoroutine(FadeOut(canvasGroup, duration, onFinished));
        }

        public static void FadeIn(this MonoBehaviour monoBehaviour, CanvasGroup canvasGroup, float duration, [CanBeNull] Action onFinished)
        {
            monoBehaviour.StartCoroutine(FadeIn(canvasGroup, duration, onFinished));
        }

        public static IEnumerator FadeOut(CanvasGroup canvasGroup, float duration, [CanBeNull] Action onFinished)
        {
            canvasGroup.DOFade(0, duration);
            yield return new WaitForSeconds(duration);
            canvasGroup.gameObject.SetActive(false);
            onFinished?.Invoke();
        }

        public static IEnumerator FadeIn(CanvasGroup canvasGroup, float duration, [CanBeNull] Action onFinished)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.DOFade(1, duration);
            yield return new WaitForSeconds(duration);
            onFinished?.Invoke();
        }
    }
}
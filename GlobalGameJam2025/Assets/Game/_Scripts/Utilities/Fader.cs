using UnityEngine;
using System;
using DG.Tweening;

public class Fader : PersistentSingleton<Fader>
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    public void FadeOut(Action onComplete)
    {
        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void FadeIn(Action onComplete)
    {
        fadeCanvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
        {
            onComplete?.Invoke();
            fadeCanvasGroup.blocksRaycasts = false;
        });
    }
}
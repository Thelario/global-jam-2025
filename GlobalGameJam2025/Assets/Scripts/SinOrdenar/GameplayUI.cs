using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private CanvasGroup blocker;
    [SerializeField] private Image mainImage;
    [Header("Sprites")]
    [SerializeField] private Sprite finishSprite;
    [SerializeField] private Sprite readySprite, setSprite, goSprite;

    private MinigameManager minigameManager;
    private bool shoudlCount = false;

    private void OnEnable()
    {
        minigameManager = MinigameManager.Instance;
        if (!minigameManager)
        {
            Debug.Log("Minigame Manager Missing!");
            return;
        }

        minigameManager.OnMinigameInit += ShowCountdown;
        minigameManager.OnMinigameStart += () => shoudlCount = true;
        minigameManager.OnMinigameEnd += ShowEnd;
    }
    protected void OnDisable()
    {
        if (!minigameManager) return;
        minigameManager.OnMinigameInit -= ShowCountdown;
        minigameManager.OnMinigameStart -= () => shoudlCount = true;
        minigameManager.OnMinigameEnd -= ShowEnd;
    }
    private void ShowCountdown()
    {
        if (!mainImage)
        {
            MinigameManager.Instance.StartMinigame();
            return;
        }
        blocker.alpha = 1.0f;
        mainImage.sprite = readySprite;
        mainImage.DOFade(0, 0);
        
        //guille me libre por este pedazo de mierdon
        Sequence introSeq = DOTween.Sequence();
        mainImage.transform.position += Vector3.up * -25;
        introSeq.Append(mainImage.DOFade(1, 0.5f))
            .Join(mainImage.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack))
            .Join(mainImage.transform.DOMoveY(mainImage.transform.position.y +25, 0.4f).SetEase(Ease.OutQuad))
            .AppendCallback(() =>
            {
                mainImage.sprite = setSprite;
                mainImage.color = new Color(1, 1, 1, 0);
                mainImage.transform.position += Vector3.up * -25;
            })
            .Append(mainImage.DOFade(1, 0.5f))
            .Join(mainImage.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack))
            .Join(mainImage.transform.DOMoveY(mainImage.transform.position.y +25, 0.4f).SetEase(Ease.OutQuad))
            .AppendCallback(() =>
            {
                mainImage.sprite = goSprite;
                mainImage.color = new Color(1, 1, 1, 0);
                mainImage.transform.position += Vector3.up * -25;
            })
            .Append(mainImage.DOFade(1, 0.5f))
            .Join(mainImage.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack))
            .Join(mainImage.transform.DOMoveY(mainImage.transform.position.y +25, 0.3f).SetEase(Ease.OutQuad))
            .OnComplete(()=>
            {
                blocker.DOFade(0, 0.1f);
                mainImage.DOFade(0, 0.1f);
                MinigameManager.Instance.StartMinigame();
            });
    }

    private void Update()
    {
        if (timerText && shoudlCount)
        {
            timerText.text = $"Timer\r\n<size=200%><color=#E7A950>{minigameManager.Timer.ToString("0")}";
        }
    }
    private void ShowEnd()
    {
        if (!mainImage || !blocker) return;

        StartCoroutine(WaitAndChangeScene());
        mainImage.sprite = finishSprite;
        blocker.DOFade(1, 0.1f);
        mainImage.DOFade(1, 0.2f);
        mainImage.transform.DOScale(1, 0.45f).SetEase(Ease.OutBack);
    }
    private IEnumerator WaitAndChangeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneNav.GoTo(SceneType.Score);
    }
}

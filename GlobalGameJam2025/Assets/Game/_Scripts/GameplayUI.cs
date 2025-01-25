using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private CanvasGroup blocker;
    [SerializeField] private Image mainImage;
    [Header("Sprites")]
    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite finishSprite;
    private MinigameManager manager;
    private bool shoudlCount = false;
    private void OnEnable()
    {
        manager = MinigameManager.Instance;
        manager.OnMinigameStart += () => shoudlCount = true;
        manager.OnMinigameEnd += ShowEnd;
    }
    private void OnDisable()
    {
        manager.OnMinigameStart -= () => shoudlCount = true;
        manager.OnMinigameEnd -= ShowEnd;
    }
    private IEnumerator Start()
    {
        if (!mainImage) yield return null;
        blocker.alpha = 1.0f;
        mainImage.sprite = readySprite;
        mainImage.DOFade(0, 0);

        yield return new WaitForSeconds(1.0f);
        mainImage.DOFade(1, 0.5f)
            .OnComplete(()=>
            {
                blocker.DOFade(0, 0.1f);
                mainImage.DOFade(0, 0.1f);
                //MinigameManager.Instance.StartMinigame();
            });
        //introText.
    }

    private void Update()
    {
        if (timerText && shoudlCount)
        {
            timerText.text = $"Timer\r\n<size=200%><color=#E7A950>{manager.GameTimer.ToString("0")}";
        }
    }
    private void ShowEnd()
    {
        if (!mainImage || !blocker) return;
        mainImage.sprite = finishSprite;
        blocker.DOFade(1, 0.1f);
        mainImage.DOFade(1, 0.2f);
        mainImage.transform.DOScale(1, 0.45f).SetEase(Ease.OutBack)
            .OnComplete(()=>SceneNav.GoTo(SceneType.Score));
    }

}

using TMPro;
using UnityEngine;

public class MinigameDurationControlManager : Singleton<MinigameDurationControlManager>
{
    public delegate void MinigameEnd();
    public MinigameEnd OnMinigameEnd;
    
    [Header("Minigame")]
    [SerializeField] private float minigameDuration = 1f;
    
    [Header("UI")]
    [SerializeField] private TMP_Text minigameDurationText;

    private bool _minigameEnded;
    private float _minigameDurationCounter;

    private void Start()
    {
        Configure();
    }

    public void Configure()
    {
        _minigameEnded = false;
        _minigameDurationCounter = minigameDuration * 60f;

        CalculateTimeText();
    }

    private void CalculateTimeText()
    {
        int minutes = Mathf.FloorToInt(_minigameDurationCounter / 60f);
        int seconds = Mathf.FloorToInt(_minigameDurationCounter % 60f);

        minigameDurationText.text = $"{minutes:00}:{seconds:00}";
    }

    private void Update()
    {
        if (_minigameEnded)
            return;
        
        _minigameDurationCounter -= Time.deltaTime;
        CalculateTimeText();

        if (!(_minigameDurationCounter <= 0f))
            return;
        
        _minigameEnded = true;
        OnMinigameEnd?.Invoke();
    }
}

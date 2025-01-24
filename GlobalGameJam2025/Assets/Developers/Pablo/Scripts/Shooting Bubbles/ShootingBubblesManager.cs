using TMPro;
using UnityEngine;

public class ShootingBubblesManager : Singleton<ShootingBubblesManager>
{
    [Header("Minigame")]
    [SerializeField] private float minigameDuration = 1f;
    
    [Header("UI")]
    [SerializeField] private TMP_Text minigameDurationText;

    private float _minigameDurationCounter;

    private void Start()
    {
        _minigameDurationCounter = minigameDuration * 60f;

        CalculateTimeText();
    }

    private void CalculateTimeText()
    {
        int minutes = Mathf.FloorToInt(_minigameDurationCounter / 60f);
        int seconds = Mathf.FloorToInt(_minigameDurationCounter % 60f);

        minigameDurationText.text = $"{minutes:00}:{seconds:00}";
    }
}

using TMPro;
using UnityEngine;

public class ShootingBubblesManager : Engine.Singleton<ShootingBubblesManager>
{
    [Header("Minigame")]
    [SerializeField] private float minigameDuration = 60f;
    
    [Header("UI")]
    [SerializeField] private TMP_Text minigameDurationText;

    private float _minigameDurationCounter;

    private void Start()
    {
        
    }
}

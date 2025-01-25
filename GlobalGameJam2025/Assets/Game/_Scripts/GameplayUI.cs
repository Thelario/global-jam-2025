using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private MinigameManager manager;
    private bool shoudlCount = false;
    private void Start()
    {
        manager = MinigameManager.Instance;
        manager.OnMinigameStart += () => shoudlCount = true;
    }
    private void Update()
    {
        if(timerText && shoudlCount) timerText.text = manager.GameTimer.ToString();
    }
}

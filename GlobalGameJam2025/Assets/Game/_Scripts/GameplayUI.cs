using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private MinigameManager manager;
    private bool shoudlCount = false;
    private void OnEnable()
    {
        manager = MinigameManager.Instance;
        manager.OnMinigameStart += () => shoudlCount = true;
    }
    private void OnDisable()
    {
        manager.OnMinigameStart -= () => shoudlCount = true;
    }
    private void Update()
    {
        if (timerText && shoudlCount)
        {
            timerText.text = $"Timer\r\n<size=200%><color=#E7A950>{manager.GameTimer.ToString("0")}";
        }
    }
}

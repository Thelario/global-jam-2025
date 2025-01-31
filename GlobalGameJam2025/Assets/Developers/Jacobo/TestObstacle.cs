using UnityEngine;

public class TestObstacle : MonoBehaviour, IMinigameEventListener
{
    public void OnMinigameEnd()
    {
        transform.localScale = Vector3.one * 8.0f;
    }

    public void OnMinigameInit()
    {
        transform.localScale = Vector3.one * 0.5f;
    }

    public void OnMinigameStart()
    {
        transform.localScale = Vector3.one * 2.0f;
    }

    public void OnPlayerDeath(PlayerCore player)
    {
        transform.localScale = Vector3.one * 4.0f;
    }
}

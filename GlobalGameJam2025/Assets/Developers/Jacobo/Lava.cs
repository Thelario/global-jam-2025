using UnityEngine;

public class Lava : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player") || triggered) return;
        triggered = true;
        MinigameManager man = MinigameManager.Instance;
        if(collision.gameObject.TryGetComponent(out MultiplayerInstance ins))
        {
            man.PlayerDeath(ins);
        }
    }
}

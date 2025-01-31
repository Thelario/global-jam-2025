using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if(collision.gameObject.TryGetComponent(out PlayerCore player))
        {
            MinigameManager.Instance.PlayerDeath(player);
        }
    }
}

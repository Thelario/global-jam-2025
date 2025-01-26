using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        MinigameManager man = MinigameManager.Instance;
        if(collision.gameObject.TryGetComponent(out MultiplayerInstance ins))
        {
            ins.GetComponent<PlayerController>().EnableRollVolume(false);
            man.PlayerDeath(ins);
        }
    }
}

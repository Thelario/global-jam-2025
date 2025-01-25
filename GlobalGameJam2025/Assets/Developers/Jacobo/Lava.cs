using UnityEngine;

public class Lava : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Te");
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("LAVA");
        if (!collision.gameObject.CompareTag("Player")) return;
        MinigameManager man = MinigameManager.Instance;
        Debug.Log("LAVA");
        if(collision.gameObject.TryGetComponent(out MultiplayerInstance ins))
        {
            Debug.Log("LAVA");
            man.PlayerDeath(ins);
        }
    }
}

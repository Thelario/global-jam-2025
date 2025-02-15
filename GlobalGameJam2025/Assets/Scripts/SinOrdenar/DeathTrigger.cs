using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var playerController) != true)
            return;
        
        SoundManager.Instance.PlaySound(Sound.BubbleExplosion);
        Destroy(other.gameObject);
    }
}

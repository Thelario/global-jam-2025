using UnityEngine;

public class BubbleCanyon : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bubbleToShoot;

    public void Shoot()
    {
        Instantiate(bubbleToShoot, shootPoint.position, shootPoint.rotation, shootPoint);
    }
}

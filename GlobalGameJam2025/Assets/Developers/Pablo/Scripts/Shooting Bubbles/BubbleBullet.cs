using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private void Update()
    {
        transform.position += transform.right * (moveSpeed * Time.deltaTime);
    }
}
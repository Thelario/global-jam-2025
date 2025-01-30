using UnityEngine;

public class Crown : MonoBehaviour
{
    public Transform playerFollow;

    static public Crown instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (playerFollow != null)
            transform.position = playerFollow.position;
    }
}

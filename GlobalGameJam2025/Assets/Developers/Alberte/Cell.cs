using UnityEngine;

public class Cell : MonoBehaviour
{
    public void Fall()
    {
        transform.localScale = Vector3.one * 0.75f;
    }
}

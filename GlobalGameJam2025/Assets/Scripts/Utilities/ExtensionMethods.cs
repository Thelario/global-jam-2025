using UnityEngine;

public static class ExtensionMethods
{
    public static void Reset(this Transform transform)
    {
        // Reset position, rotation, and scale
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        transform.localScale = Vector3.one;
    }
}

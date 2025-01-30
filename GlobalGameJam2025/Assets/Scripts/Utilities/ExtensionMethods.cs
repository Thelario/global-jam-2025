using UnityEngine;

public static class ExtensionMethods
{
    public static void Reset(this Transform transform)
    {
        // Reset position, rotation, and scale
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        transform.localScale = Vector3.one;
    }
    public static Vector2 WorldToCanvas(this Canvas canvas, Vector3 world_position, Camera camera = null)
    {
        if (camera == null) camera = Camera.main;

        Vector3 screenPos = camera.WorldToScreenPoint(world_position);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, camera, out localPoint);

        return localPoint;
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddUIShadow
{
    private const int moveAmm = 10;

    [MenuItem("CONTEXT/RectTransform/Add Shadow")]
    private static void ResetRectTransform(MenuCommand command)
    {
        RectTransform parentRect = (RectTransform)command.context;

        GameObject child = new GameObject("DropShadow");
        child.transform.SetParent(parentRect);

        // Set RectTransform properties to match parent
        RectTransform childRect = child.AddComponent<RectTransform>();
        childRect.anchorMin = parentRect.anchorMin;
        childRect.anchorMax = parentRect.anchorMax;
        childRect.pivot = parentRect.pivot;
        childRect.sizeDelta = parentRect.sizeDelta;
        // Set child to local position zero
        childRect.localPosition = Vector3.zero;
        childRect.localRotation = Quaternion.identity;
        childRect.localScale = Vector3.one;

        childRect.anchoredPosition += new Vector2(moveAmm, -moveAmm);
        Image childIMG = child.AddComponent<Image>();
        childIMG.raycastTarget = false;
        childIMG.maskable = false;
        
        Selection.activeGameObject = child;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private Image controllerIcon;
    [SerializeField] private Image backgroundColor;
    private CanvasGroup cg;
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }
    public void SetProfile(float al)=> cg.alpha = al;
    public void SetProfile(float alpha, PlayerData data)
    {
        cg.alpha = alpha;
        controllerIcon.sprite = AssetLocator.GetControllerIcon(data.GetDeviceType());
        backgroundColor.color = data.GetSkin().mainColor;
    }
}

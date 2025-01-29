using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private Image controllerIcon;
    [SerializeField] private Image backgroundColor;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private Button removeButton;
    private CanvasGroup cg;
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        if (removeButton) removeButton.onClick.AddListener(TryRemovePlayer);
    }
    private void TryRemovePlayer()
    {
        GameManager.Instance.RemovePlayer(0);
    }
    private void OnDisable()
    {
        if (removeButton) removeButton.onClick.RemoveAllListeners();
    }
    public void SetProfile(float al)=> cg.alpha = al;
    public void SetProfile(float alpha, PlayerData data)
    {
        cg.alpha = alpha;
        controllerIcon.sprite = AssetLocator.GetControllerIcon(data.GetDeviceType());
        backgroundColor.color = data.GetSkin().mainColor;
        playerText.text = $"Player {GameManager.Instance.GetPlayerIndex(data)+1}";
    }
}

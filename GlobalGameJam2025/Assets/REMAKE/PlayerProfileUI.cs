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

    private Color initBackColor;
    private CanvasGroup cg;
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        initBackColor = backgroundColor.color;
        if (removeButton) removeButton.onClick.AddListener(TryRemovePlayer);
        
        SetProfileEmpty();
    }
    private void TryRemovePlayer()
    {
        GameManager.Instance.RemovePlayer(0);
    }
    private void OnDisable()
    {
        if (removeButton) removeButton.onClick.RemoveAllListeners();
    }
    //Para cuando se desconecte un mando
    public void SetProfileEmpty()
    {
        cg.alpha = 0.2f;
        backgroundColor.color = initBackColor;
        playerText.text = "Press Start";
    }
    //Para cuando se Conecte un mando
    public void SetProfile(float alpha, PlayerData data)
    {
        cg.alpha = alpha;
        controllerIcon.sprite = AssetLocator.GetControllerIcon(data.GetDeviceType());
        backgroundColor.color = data.GetSkin().mainColor;
        playerText.text = $"Player {GameManager.Instance.GetPlayerIndex(data)+1}";
    }
}

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
    private int lastIndex = -1;
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        initBackColor = backgroundColor.color;
        if (removeButton) removeButton.onClick.AddListener(TryRemovePlayer);
        
        SetProfileEmpty();
    }
    private void TryRemovePlayer()
    {
        if(lastIndex != -1) GameManager.Instance.RemovePlayer(lastIndex);
    }
    private void OnDisable()
    {
        if (removeButton) removeButton.onClick.RemoveAllListeners();
    }
    //Para cuando se desconecte un mando
    public void SetProfileEmpty()
    {
        lastIndex = -1;
        cg.alpha = 0.2f;
        backgroundColor.color = initBackColor;
        playerText.text = "Press Start";
    }
    //Para cuando se Conecte un mando
    public void SetProfile(float alpha, PlayerData data)
    {
        cg.alpha = alpha;
        int index = GameManager.Instance.GetPlayerIndex(data);
        lastIndex = index;
        controllerIcon.sprite = AssetLocator.GetControllerIcon(data.GetDeviceType());
        backgroundColor.color = data.GetSkin().mainColor;
        playerText.text = $"Player {index + 1}";
    }
}

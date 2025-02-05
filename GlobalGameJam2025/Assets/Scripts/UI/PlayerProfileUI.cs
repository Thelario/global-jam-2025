using DG.Tweening;
using System;
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
    private int assignedIndex = -1;
    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        initBackColor = backgroundColor.color;
        if (removeButton) removeButton.onClick.AddListener(TryRemovePlayer);
        
        SetProfileEmpty();
        StartTween();
    }

    private void StartTween()
    {
        transform.DOScale(Vector3.one * 1.025f, 0.3f)
            .SetDelay(UnityEngine.Random.Range(0.2f, 0.7f))
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void TryRemovePlayer()
    {
        if (assignedIndex == -1) return;
        GameManager.Instance.RemovePlayer(GameManager.Instance.GetPlayer(assignedIndex));
    }
    private void OnDisable()
    {
        if (removeButton) removeButton.onClick.RemoveAllListeners();
    }
    //Para cuando se desconecte un mando
    public void SetProfileEmpty()
    {
        assignedIndex = -1;
        cg.alpha = 0.2f;
        backgroundColor.color = initBackColor;
        playerText.text = "Press Start";
    }
    //Para cuando se Conecte un mando
    public void SetProfile(float alpha, PlayerData data)
    {
        cg.alpha = alpha;
        assignedIndex = GameManager.Instance.GetPlayerIndex(data);
        controllerIcon.sprite = AssetLocator.GetControllerIcon(data.GetDeviceType());
        backgroundColor.color = data.GetSkin().mainColor;
        playerText.text = $"Player {assignedIndex + 1}";
    }
}

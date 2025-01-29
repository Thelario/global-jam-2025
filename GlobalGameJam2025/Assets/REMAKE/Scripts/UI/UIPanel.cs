using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    private static readonly Dictionary<Type, UIPanel> m_panels = new Dictionary<Type, UIPanel>();

    protected event UnityAction onBeforeShow;
    protected event UnityAction onAfterShow;
    protected event UnityAction onHide;

    protected CanvasGroup canvasGroup;
    protected bool isVisible;
    protected Tween fadeTween;
    [SerializeField] protected float fadeTime = 0.4f;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        isVisible = false;
        gameObject.SetActive(false);

        var panelType = GetType();
        if (!m_panels.ContainsKey(panelType))
        {
            m_panels.Add(panelType, this);
        }
    }
    
    protected virtual void OnDestroy()
    {
        var panelType = GetType();
        if (m_panels.ContainsKey(panelType))
        {
            m_panels.Remove(panelType);
        }
    }
    
    
    public virtual void Show()
    {
        if (isVisible) return;

        foreach(var panel in m_panels)
        {
            panel.Value.Hide();
        }
        if (fadeTween != null) fadeTween.Kill();

        gameObject.SetActive(true);
        isVisible = true;
        onBeforeShow?.Invoke();
        fadeTween = canvasGroup.DOFade(1, fadeTime).OnComplete(() => onAfterShow?.Invoke());
    }

    public virtual void Hide(bool instant = false)
    {
        if (!isVisible) return;

        if (fadeTween != null) fadeTween.Kill();

        isVisible = false;

        fadeTween = canvasGroup.DOFade(0, fadeTime*0.25f).OnComplete(() =>
        {
            onHide?.Invoke();
            gameObject.SetActive(false);
        });
    }

    public bool IsVisible() => isVisible;

    public static List<UIPanel> GetAllPanels() => new List<UIPanel>(m_panels.Values);

    public static UIPanel GetPanel(Type panelType)
    {
        if (m_panels.TryGetValue(panelType, out UIPanel panel)) return panel;
        else
        {
            Debug.LogWarning($"Tried to access non-existing panel: {panelType}");
            return null;
        }
    }
}

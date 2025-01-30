using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
{
    Tween t;
    [Serializable]
    public enum FXType
    {
        Scale,
        Move,
    }
    [Serializable]
    public enum FXAmmount
    {
        Small,
        Medium,
        Big,
    }

    public FXType fxType = FXType.Move;
    public FXAmmount fxAmmount = FXAmmount.Medium;
    public bool useChild = true;

    //private Image img;
    private Transform tr;
    private RectTransform rt;
    private bool mouseOver = false;

    public event Action OnEnter;
    public event Action OnExit;

    //SCALE TYPE
    [SerializeField] private float scaleUpAmmount = 1.035f;
    public float ScaleUpAmmount
    {
        get { return scaleUpAmmount; }
        set { scaleUpAmmount = value; }
    }

    //MOVE TYPE
    [SerializeField] private int moveAmmount = 5;
    public int MoveAmmount
    {
        get { return moveAmmount; }
        set { moveAmmount = value; }
    }

    private Vector2 initRectPos, targetRectPos;

    private void OnEnable()
    {
        if (!useChild || transform.childCount == 0) tr = transform;
        else tr = transform.GetChild(0);
        rt = tr.GetComponent<RectTransform>();
        
        tr.localScale = Vector3.one;
        initRectPos = rt.anchoredPosition;
        targetRectPos = initRectPos + new Vector2(-moveAmmount, moveAmmount);
    }

    public void OnSelect(BaseEventData eventData = null)
    {
        if (t != null) return;//t.Kill();
        OnEnter?.Invoke();
        if (fxType == FXType.Scale)
        {
            t = tr.DOScale(Vector3.one * (scaleUpAmmount), 0.1f).SetEase(Ease.OutQuad);
        }
        else
        {
            t = rt.DOAnchorPos(targetRectPos, 0.15f).SetEase(Ease.OutQuad);
        }
        t.OnComplete(() =>
        {
            t = null;
            if (!mouseOver) OnDeselect();
        });
    }

    public void OnDeselect(BaseEventData eventData = null)
    {
        if (t != null) return;//t.Kill();
        OnExit?.Invoke();
        if (fxType == FXType.Scale)
        {
            t = tr.DOScale(Vector3.one, 0.075f);
        }
        else
        {
            t = rt.DOAnchorPos(initRectPos, 0.1f).SetEase(Ease.OutQuad);
        }
        t.OnComplete(() =>
        {
            t = null;
            if (mouseOver) OnSelect();
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        eventData.selectedObject = null;
    }

    private void OnDisable()
    {
        if (t != null) t.Kill();
        ResetAll();
    }

    private void ResetAll()
    {
        if (t != null) t.Kill();
        t = null;
        tr.localScale = Vector3.one;
        if (fxType == FXType.Move) rt.anchoredPosition = initRectPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (t != null) t.Kill();
        t = tr.DOScale(0.925f, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetAll();
    }
}

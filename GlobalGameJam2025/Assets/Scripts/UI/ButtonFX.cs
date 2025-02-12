using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
{
    [Serializable] public enum FXType { Scale, Move }
    [Serializable] public enum FXAmmount { Small, Medium, Big }

    public FXType fxType = FXType.Move;
    public FXAmmount fxAmmount = FXAmmount.Medium;
    public bool useChild = true;

    public event Action OnEnter;
    public event Action OnExit;

    // Scale type
    [SerializeField] private float scaleUpAmmount = 1.035f;
    public float ScaleUpAmmount
    {
        get { return scaleUpAmmount; }
        set { scaleUpAmmount = value; }
    }

    // Move type
    [SerializeField] private int moveAmmount = 5;
    public int MoveAmmount
    {
        get { return moveAmmount; }
        set { moveAmmount = value; }
    }

    private bool _mouseOver = false;
    private Vector2 _initRectPos;
    private Vector2 _targetRectPos;
    private Tween _t;
    private Transform _tr;
    private RectTransform _rt;

    private void OnEnable()
    {
        if (!useChild || transform.childCount == 0) {
            _tr = transform;
        }
        else {
            _tr = transform.GetChild(0);
        }

        _rt = _tr.GetComponent<RectTransform>();
        _tr.localScale = Vector3.one;
        _initRectPos = _rt.anchoredPosition;
        _targetRectPos = _initRectPos + new Vector2(-moveAmmount, moveAmmount);
    }

    public void OnSelect(BaseEventData eventData = null)
    {
        if (_t != null) {
            return; //t.Kill();
        }

        OnEnter?.Invoke();

        if (fxType == FXType.Scale)
        {
            _t = _tr.DOScale(Vector3.one * (scaleUpAmmount), 0.1f).SetEase(Ease.OutQuad);
        }
        else
        {
            _t = _rt.DOAnchorPos(_targetRectPos, 0.15f).SetEase(Ease.OutQuad);
        }

        _t.OnComplete(() =>
        {
            _t = null;

            if (!_mouseOver) {
                OnDeselect();
            }
        });
    }

    public void OnDeselect(BaseEventData eventData = null)
    {
        if (_t != null) {
            return; //t.Kill();
        }

        OnExit?.Invoke();

        if (fxType == FXType.Scale) {
            _t = _tr.DOScale(Vector3.one, 0.075f);
        }
        else {
            _t = _rt.DOAnchorPos(_initRectPos, 0.1f).SetEase(Ease.OutQuad);
        }

        _t.OnComplete(() =>
        {
            _t = null;

            if (_mouseOver) {
                OnSelect();
            }
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOver = true;
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOver = false;
        eventData.selectedObject = null;
    }

    private void OnDisable()
    {
        if (_t != null) {
            _t.Kill();
        }

        ResetAll();
    }

    private void ResetAll()
    {
        if (_t != null) {
            _t.Kill();
        }

        _t = null;
        _tr.localScale = Vector3.one;

        if (fxType == FXType.Move) {
            _rt.anchoredPosition = _initRectPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_t != null) {
            _t.Kill();
        }

        _t = _tr.DOScale(0.925f, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetAll();
    }
}

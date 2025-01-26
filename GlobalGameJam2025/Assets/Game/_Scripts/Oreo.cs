using DG.Tweening;
using UnityEngine;

public class Oreo : MonoBehaviour
{
    [HideInInspector] public bool isAvailable = true;
    Sequence fallSeq;
    Vector3 initPos;
    private void Start()
    {
        initPos = transform.localPosition;
    }
    public void FallOreo()
    {
        if(fallSeq != null) fallSeq.Kill();
        isAvailable = false;
        Material mat = transform.GetComponent<Renderer>().material;
        fallSeq = DOTween.Sequence();
        fallSeq.Append(mat.DOFloat(0.9f, "_Sat", 0.2f).SetLoops(2, LoopType.Yoyo))
            .Join(transform.DOScale(transform.localScale * 0.95f, 0.2f).SetLoops(2, LoopType.Yoyo))
            .Append(transform.DOMoveY(-5.5f, 2.0f).SetEase(Ease.InOutBack))
            .Append(transform.DOMoveY(initPos.y, 1f))
            .OnComplete(() => { isAvailable = true; });
    }
}

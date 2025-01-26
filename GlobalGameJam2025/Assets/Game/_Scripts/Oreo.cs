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
        fallSeq.Append(mat.DOFloat(0.95f, "_Sat", 0.15f))
            .Insert(0.3f, mat.DOFloat(0f, "_Sat", 0.1f))
            .Join(transform.DOScale(transform.localScale * 0.95f, 0.2f).SetLoops(2, LoopType.Yoyo))
            .Append(transform.DOMoveY(-5.5f, 4.0f))
            .Append(transform.DOMoveY(initPos.y, 1f))
            .OnComplete(() => { isAvailable = true; });
    }
}

using DG.Tweening;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool falling = false;
    Sequence fallSeq;
    public void Fall()
    {
        if(falling) return;
        falling = true;
        fallSeq = DOTween.Sequence();
        Material mat = transform.GetComponent<Renderer>().material;
        fallSeq.Append(mat.DOFloat(0.85f, "_Sat", 0.15f).SetLoops(2, LoopType.Yoyo))
            .Join(transform.DOScale(Vector3.one * 1.075f,0.15f).SetLoops(2, LoopType.Yoyo))
            .PrependInterval(Random.Range(0f,0.2f))
            .AppendCallback(() => FallDown());
    }
    public void FallDown()
    {
        transform.DOScale(Vector3.zero, 0.45f).SetEase(Ease.InBack)
            .OnComplete(() => 
            {
                if(fallSeq != null) fallSeq.Kill();
                Destroy(gameObject); 
            });
        
    }
}

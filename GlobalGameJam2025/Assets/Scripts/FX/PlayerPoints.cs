using DG.Tweening;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    Sequence t;
    public void Init(int points)
    {
        Renderer rend = GetComponent<Renderer>();
        
        if (!rend) return;
        t = DOTween.Sequence();
        transform.localScale = Vector3.zero;
        rend.material.SetVector("_Offset", new Vector2((points)*0.25f, 0));
        rend.material.DOFade(0, 0f);

        t.Append(transform.DOLocalMove(transform.position + transform.up, 0.9f))
            .Join(rend.material.DOFade(1, 0.15f))
            .Join(transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack))
            .Insert(0.7f, rend.material.DOFade(0, 0.15f))
            .OnComplete(()=> Destroy(gameObject));
    }
    private void OnDestroy() => t?.Kill();
}

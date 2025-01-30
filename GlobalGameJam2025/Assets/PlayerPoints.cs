using DG.Tweening;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    Tween t;
    public void Init(int points)
    {
        Debug.Log("A");
        transform.GetComponent<Renderer>().material.SetVector("_Offset", new Vector2(points*0.25f, 0));
        transform.localScale = Vector3.zero;
        t = transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
            .SetLoops(2, LoopType.Yoyo);
    }
    private void OnDestroy() => t?.Kill();
}

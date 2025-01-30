using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class IconFX : MonoBehaviour
{
    public bool onlyScale = false;
    private Tween t1, t2;
    [SerializeField] private float scaleMod = 1.25f;
    [SerializeField] private float scaleTime = 1.25f;
    [SerializeField] private Vector3 rotVec = new Vector3(-2, 0, 20);
    void Start()
    {
        t1 = transform.DOScale(Vector3.one * scaleMod, scaleTime).SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        if (onlyScale)return;
        t2 = transform.DOLocalRotate(rotVec, 1.2f).SetEase(Ease.InOutSine)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnDestroy()
    {
        if(t1 != null ) t1.Kill();//TODO: Auto en scene Change
        if(t2 != null ) t2.Kill();//TODO: Auto en scene Change
    }
}

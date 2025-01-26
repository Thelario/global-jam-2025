using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PinballCylinder : MonoBehaviour
{
    Tween t, shaderT;
    [SerializeField] private float pinballForce;
    [SerializeField] private float popupTime;
    [SerializeField] private float scaleMultiplier;
    [SerializeField] private Ease ease = Ease.OutQuad;
    [SerializeField] private Transform target;
    [SerializeField] private List<Sound> sounds;    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var playerController) != true)
            return;

        Vector3 hitDirection = other.transform.position - other.contacts[0].point;

        SoundManager.Instance.PlaySound(Sound.BubbleGiggly);
        playerController.SetLinearVelocity(hitDirection * pinballForce);
        PopUp();
    }

    private void PopUp()
    {
        if (t != null) t.Kill();
        if (shaderT != null) shaderT.Kill();
        t = target.DOScale(Vector3.one * scaleMultiplier, popupTime).SetEase(ease).SetLoops(2, LoopType.Yoyo);

        if (!target.TryGetComponent<Renderer>(out var renderer)) return;
        shaderT = renderer.material.DOFloat(0.85f, "_Sat", 0.2f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
    }
}

using DG.Tweening;
using UnityEngine;

public class PinballCylinder : MonoBehaviour
{
    [SerializeField] private float pinballForce;
    [SerializeField] private float popupTime;
    [SerializeField] private float scaleMultiplier;
    [SerializeField] private Ease ease;
    [SerializeField] private Transform target;
    
    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = target.localScale;
    }

    private void OnEnable()
    {
        _originalScale = target.localScale;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var playerController) != true)
            return;

        Vector3 hitDirection = other.transform.position - other.contacts[0].point;

        playerController.SetLinearVelocity(hitDirection * pinballForce);
        PopUp();
    }

    private void PopUp()
    {
        _originalScale = target.localScale;
        Vector3 newScale = new Vector3(_originalScale.x * scaleMultiplier, _originalScale.y,
            _originalScale.z * scaleMultiplier);
        
        target.DOKill();
        target.DOScale(newScale, popupTime).SetEase(ease).OnComplete(Reset);
    }

    private void Reset()
    {
        target.DOKill();
        target.DOScale(_originalScale, popupTime / 2f).onComplete();
    }
}

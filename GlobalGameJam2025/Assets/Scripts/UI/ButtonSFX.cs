using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Sound selectButtonSound = Sound.SelectButton;

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.Instance.PlaySound(selectButtonSound);
    }
}

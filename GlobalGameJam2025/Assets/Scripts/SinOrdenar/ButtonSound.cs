using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sound soundToPlayOnClick;
    [SerializeField] private Sound soundToPlayOnMouseEnter;
    [SerializeField] private Sound soundToPlayOnMouseExit;
    
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlaySoundOnClick);
    }

    private void PlaySoundOnClick()
    {
        if (soundToPlayOnClick != Sound.None)
            return;
        
        print("PlaySoundOnClick");
        SoundManager.Instance.PlaySound(soundToPlayOnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (soundToPlayOnMouseEnter != Sound.None)
            return;
        
        print("OnPointerEnter");
        SoundManager.Instance.PlaySound(soundToPlayOnMouseEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (soundToPlayOnMouseExit != Sound.None)
            return;
        
        print("OnPointerExit");
        SoundManager.Instance.PlaySound(soundToPlayOnMouseExit);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private Sound soundToPlayOnClick;
    
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlaySoundOnClick);
    }

    private void PlaySoundOnClick()
    {
        SoundManager.Instance.PlaySound(soundToPlayOnClick);
    }
}

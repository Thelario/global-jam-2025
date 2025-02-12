using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private bool canPresss = false;

    private bool _canPress = false;
    private static bool _playMenuMusic = true;

    private void Awake()
    {
        if (_playMenuMusic) {
            SoundManager.Instance.PlaySound(Sound.MenuMusic);
            _playMenuMusic = false;
        }  
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        _canPress = canPresss;
    }

    private void Update()
    {
        if (_canPress && Input.anyKey)
        {
            SceneNav.GoTo(SceneType.PlayerSelect);
        }
    }
}

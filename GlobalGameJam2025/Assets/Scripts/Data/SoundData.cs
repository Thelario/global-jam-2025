using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Sound Data SO", order = 2)]
public class SoundData : ScriptableObject
{
    [SerializeField] private float defaultVolume = 2.0f;
    [SerializeField] private float defaultPitch = 1.0f;
    [SerializeField] private float defaultPitchModifier = 0.2f;
    [SerializeField] private float playerMoveTimerMax = .25f;
    [SerializeField] private GameObject defaultSfxAudioSource;
    [SerializeField] private GameObject defaultMusicAudioSource;
    
    public float DefaultVolume => defaultVolume;
    public float DefaultPitch => defaultPitch;
    public float DefaultPitchModifier => defaultPitchModifier;
    public float PlayerMoveTimerMax => playerMoveTimerMax;
    public GameObject DefaultSfxAudioSource => defaultSfxAudioSource;
    public GameObject MusicAudioSource => defaultMusicAudioSource;
}

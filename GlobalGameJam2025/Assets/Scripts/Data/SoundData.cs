using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Sound Data SO", order = 2)]
public class SoundData : ScriptableObject
{
    public float defaultPitch = 1.0f;
    public float defaultPitchModifier = 0.2f;
    public float playerMoveTimerMax = .25f;
    public GameObject defaultSfxAudioSource;
    public GameObject defaultMusicAudioSource;
}

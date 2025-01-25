using UnityEngine;

public enum Sound { None = 0, BubbleHit = 1, BubbleDeep = 2 };

[CreateAssetMenu(fileName = "SO_Sound_", menuName = "ScriptableObjects/SoundSO", order = 1)]
public class SoundSO : ScriptableObject
{
    [SerializeField] private Sound sound;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volumeModifier;

    public Sound Sound => sound;
    public AudioClip Clip => clip;
    public float VolumeModifier => volumeModifier;
}

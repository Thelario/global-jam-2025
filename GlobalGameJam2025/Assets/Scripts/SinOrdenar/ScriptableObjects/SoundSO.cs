using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    None = 0, MenuMusic = 6, GameMusic = 7, BubbleHit = 1, BubbleDeep = 2, BubbleGiggly = 3, BubbleRoll = 4,
    BubbleExplosion = 5, SelectButton = 8, DeselectButton = 9
};

[CreateAssetMenu(fileName = "SO_Sound_", menuName = "ScriptableObjects/SoundSO", order = 1)]
public class SoundSO : ScriptableObject
{
    [SerializeField] private Sound sound;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volumeModifier;
    [SerializeField] private List<AudioClip> otherClips;

    public Sound Sound => sound;
    public AudioClip Clip => clip;
    public float VolumeModifier => volumeModifier;
    public List<AudioClip> OtherClips => otherClips;
}

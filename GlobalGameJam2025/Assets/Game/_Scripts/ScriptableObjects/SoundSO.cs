using UnityEngine;

[CreateAssetMenu(fileName = "SO_Sound_", menuName = "ScriptableObjects/SoundSO", order = 1)]
public class SoundSO : ScriptableObject
{
    [SerializeField] private Sound sound;
    [SerializeField] private AudioClip clip;

    public Sound Sound => sound;
    public AudioClip Clip => clip;
}

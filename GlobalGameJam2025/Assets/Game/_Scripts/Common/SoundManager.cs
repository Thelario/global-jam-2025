using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private string soundsLoadPath;
    [SerializeField] private float defaultVolume;
    [SerializeField] private float defaultPitch;
    [SerializeField] private float defaultPitchModifier;
    [SerializeField] private AudioSource defaultAudioSource;

    private Dictionary<Sound, SoundSO> _sounds;

    protected override void Awake()
    {
        base.Awake();

        Configure();
    }

    private void Configure()
    {
        _sounds = new Dictionary<Sound, SoundSO>();

        SoundSO[] sounds = Resources.LoadAll<SoundSO>(soundsLoadPath);

        foreach (SoundSO sound in sounds) {
            _sounds.Add(sound.Sound, sound);
        }
    }

    public void PlaySound(Sound sound)
    {
        _sounds.TryGetValue(sound, out SoundSO clip);

        if (clip == null) {
            Debug.LogError("The sound " + sound.ToString() + " couldn't be found.");
            return;
        }

        defaultAudioSource.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        defaultAudioSource.PlayOneShot(clip.Clip, defaultVolume * clip.VolumeModifier);
    }

    public void PlaySound(Sound sound, AudioSource source)
    {
        _sounds.TryGetValue(sound, out SoundSO clip);

        if (clip == null) {
            Debug.LogError("The sound " + sound.ToString() + " couldn't be found.");
            return;
        }

        source.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        source.PlayOneShot(clip.Clip, defaultVolume * clip.VolumeModifier);
    }
}

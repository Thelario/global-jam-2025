using System.Collections.Generic;
using UnityEngine;

public enum Sound { None = 0 };

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private string soundsLoadPath;
    [SerializeField] private float defaultVolume;
    [SerializeField] private float defaultPitch;
    [SerializeField] private float defaultPitchModifier;
    [SerializeField] private AudioSource defaultAudioSource;

    private Dictionary<Sound, AudioClip> _sounds;

    protected override void Awake()
    {
        base.Awake();

        Configure();
    }

    private void Configure()
    {
        _sounds = new Dictionary<Sound, AudioClip>();

        SoundSO[] sounds = Resources.LoadAll<SoundSO>(soundsLoadPath);

        foreach (SoundSO sound in sounds) {
            _sounds.Add(sound.Sound, sound.Clip);
        }
    }

    public void PlaySound(Sound sound)
    {
        _sounds.TryGetValue(sound, out AudioClip clip);

        if (clip == null) {
            Debug.LogError("The sound " + sound.ToString() + " couldn't be found.");
            return;
        }

        defaultAudioSource.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        defaultAudioSource.PlayOneShot(clip, defaultVolume);
    }

    public void PlaySound(Sound sound, float volume)
    {
        _sounds.TryGetValue(sound, out AudioClip clip);

        if (clip == null) {
            Debug.LogError("The sound " + sound.ToString() + " couldn't be found.");
            return;
        }

        defaultAudioSource.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        defaultAudioSource.PlayOneShot(clip, defaultVolume * volume);
    }

    public void PlaySound(Sound sound, AudioSource source)
    {
        _sounds.TryGetValue(sound, out AudioClip clip);

        if (clip == null) {
            Debug.LogError("The sound " + sound.ToString() + " couldn't be found.");
            return;
        }

        source.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        source.PlayOneShot(clip, defaultVolume);
    }

    public void PlaySound(Sound sound, AudioSource source, float volume)
    {
        _sounds.TryGetValue(sound, out AudioClip clip);

        if (clip == null) {
            Debug.LogError("The sound " + sound.ToString() + " couldn't be found.");
            return;
        }

        source.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        source.PlayOneShot(clip, defaultVolume * volume);
    }
}

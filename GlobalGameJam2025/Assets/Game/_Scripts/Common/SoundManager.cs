using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private string soundsLoadPath;
    [SerializeField] private float defaultVolume;
    [SerializeField] private float defaultPitch;
    [SerializeField] private float defaultPitchModifier;
    [SerializeField] private float playerMoveTimerMax = .25f;
    [SerializeField] private AudioSource defaultAudioSource;

    private Dictionary<Sound, SoundSO> _sounds;
    private Dictionary<Sound, float> _soundTimerDictionary;

    protected override void Awake()
    {
        base.Awake();

        Configure();
    }

    private void Update()
    {
        PlaySound(Sound.BubbleRoll);
    }

    private void Configure()
    {
        _sounds = new Dictionary<Sound, SoundSO>();

        SoundSO[] sounds = Resources.LoadAll<SoundSO>(soundsLoadPath);

        foreach (SoundSO sound in sounds) {
            _sounds.Add(sound.Sound, sound);
        }
        
        // Configure the timer dictionary
        
        _soundTimerDictionary = new Dictionary<Sound, float> {
            [Sound.BubbleRoll] = 0f
        };
    }
    
    private bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default: {
                return true;
            }
            case Sound.BubbleRoll: {
                if (!_soundTimerDictionary.TryGetValue(sound, out var lastTimePlayed))
                    return true;

                if (!(lastTimePlayed + playerMoveTimerMax < Time.time))
                    return false;
                    
                _soundTimerDictionary[sound] = Time.time;
                return true;
            }
        }
    }

    public void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound) == false) {
            return;
        }
        
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
        if (CanPlaySound(sound) == false) {
            return;
        }
        
        _sounds.TryGetValue(sound, out SoundSO clip);

        if (clip == null) {
            Debug.LogError("The sound " + sound.ToString() + " couldn't be found.");
            return;
        }

        source.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        source.PlayOneShot(clip.Clip, defaultVolume * clip.VolumeModifier);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : PersistentSingleton<SoundManager>
{
    //Se podria inicializar esto, o cargar los settings de algun sitio
    private string soundsLoadPath = "Sounds";
    [SerializeField] private float defaultVolume;
    [SerializeField] private float defaultPitch;
    [SerializeField] private float defaultPitchModifier;
    [SerializeField] private float playerMoveTimerMax = .25f;
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    private Dictionary<Sound, SoundSO> _sounds;
    private Dictionary<Sound, float> _soundTimerDictionary;

    protected override void Awake()
    {
        base.Awake();

        Configure();
    }

    private void Update()
    {
        
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

    public void PlaySound(Sound sound, float volumeModifier = 1f)
    {
        if (CanPlaySound(sound) == false) {
            return;
        }
        
        _sounds.TryGetValue(sound, out SoundSO clip);
        
        if (clip == null) {
            Debug.LogError("The sound " + sound + " couldn't be found.");
            return;
        }

        // Check if the sound to play is a music clip
        
        if (sound is Sound.GameMusic or Sound.MenuMusic)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = clip.Clip;
            musicAudioSource.PlayDelayed(1f);
            return;
        }
        
        AudioClip clipToPlay =
              clip.OtherClips != null && clip.OtherClips.Count > 0
            ? clip.OtherClips[Random.Range(0, clip.OtherClips.Count)]
            : clip.Clip;
        
        // If not a music, modify pitch and play the sound

        defaultAudioSource.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        defaultAudioSource.PlayOneShot(clipToPlay, defaultVolume * clip.VolumeModifier * volumeModifier);
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
        
        if (sound is Sound.GameMusic or Sound.MenuMusic)
        {
            source.Stop();
            source.clip = clip.Clip;
            source.PlayDelayed(1f);
            return;
        }

        source.pitch = Random.Range(defaultPitch - defaultPitchModifier, defaultPitch + defaultPitchModifier);
        source.PlayOneShot(clip.Clip, defaultVolume * clip.VolumeModifier);
    }
}

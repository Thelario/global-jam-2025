using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : PersistentSingleton<SoundManager>
{
    private string _soundsLoadPath = "Sounds";
    private string _soundsDataLoadPath = "Config/SoundDataSO";
    
    private SoundData _soundData;
    
    private AudioSource _defaultAudioSource;
    private AudioSource _musicAudioSource;

    private Dictionary<Sound, SoundSO> _sounds;
    private Dictionary<Sound, float> _soundTimerDictionary;

    protected override void Awake()
    {
        base.Awake();

        Configure();
    }

    private void Configure()
    {
        // Configure all the necessary data for the SoundManager to properly work.

        _soundData = Resources.Load<SoundData>(_soundsDataLoadPath);

        if (_soundData == null) {
            Debug.LogError("The SoundData couldn't be properly loaded!!!");
        }

        bool playGameplayMusic = false;
        if (transform.childCount == 0)
        {
            // This case might occur when we want to play a minigame directly from gameplay, for example.

            gameObject.AddComponent<AudioListener>();

            if (_soundData != null)
            {
                _defaultAudioSource = Instantiate(_soundData.DefaultSfxAudioSource, transform).GetComponent<AudioSource>();
                _musicAudioSource = Instantiate(_soundData.MusicAudioSource, transform).GetComponent<AudioSource>();

                playGameplayMusic = true;
            }
            else
            {
                Debug.LogError("Audio sources couldn't be created, so they will be null!");
            }
        }
        else
        {
            // This is the default case of the game, in which the SoundManager prefab will be present in
            // the MainMenu scene from the beginning of the game.

            _defaultAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
            _musicAudioSource = transform.GetChild(1).GetComponent<AudioSource>();
        }

        // Load all the sounds.
        
        _sounds = new Dictionary<Sound, SoundSO>();

        SoundSO[] sounds = Resources.LoadAll<SoundSO>(_soundsLoadPath);

        foreach (SoundSO sound in sounds) {
            _sounds.Add(sound.Sound, sound);
        }
        
        // Configure the timer dictionary
        
        _soundTimerDictionary = new Dictionary<Sound, float> {
            [Sound.BubbleRoll] = 0f
        };

        // If we want to directly play from gameplay (or other scene), we play the music.

        if (playGameplayMusic) {
            PlaySound(Sound.GameMusic);
        }
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

                if (!(lastTimePlayed + _soundData.PlayerMoveTimerMax < Time.time))
                    return false;
                    
                _soundTimerDictionary[sound] = Time.time;
                return true;
            }
        }
    }

    /// <summary>
    /// A direct and easy approach to playing any sound at any time.
    /// </summary>
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
            _musicAudioSource.Stop();
            _musicAudioSource.clip = clip.Clip;
            _musicAudioSource.PlayDelayed(1f);
            return;
        }
        
        AudioClip clipToPlay =
              clip.OtherClips != null && clip.OtherClips.Count > 0
            ? clip.OtherClips[Random.Range(0, clip.OtherClips.Count)]
            : clip.Clip;
        
        // If not a music, modify pitch and play the sound

        _defaultAudioSource.pitch = Random.Range(_soundData.DefaultPitch - _soundData.DefaultPitchModifier, _soundData.DefaultPitch + _soundData.DefaultPitchModifier);
        _defaultAudioSource.PlayOneShot(clipToPlay, _soundData.DefaultVolume * clip.VolumeModifier * volumeModifier);
    }

    /// <summary>
    /// Call this method when you want to play sounds using a customm audio source.
    /// </summary>
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

        source.pitch = Random.Range(_soundData.DefaultPitch - _soundData.DefaultPitchModifier, _soundData.DefaultPitch + _soundData.DefaultPitchModifier);
        source.PlayOneShot(clip.Clip, _soundData.DefaultVolume * clip.VolumeModifier);
    }
}

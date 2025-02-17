using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : PersistentSingleton<SoundManager>
{
    private string _soundsLoadPath = "Sounds";
    private string _soundsDataLoadPath = "Config/SoundDataSO";

    private SoundData _soundData;
    private OptionsData _optionsData;

    private AudioSource _defaultAudioSource;
    private AudioSource _musicAudioSource;

    private Dictionary<Sound, SoundSO> _sounds;
    private Dictionary<Sound, float> _soundTimerDictionary;

    public void SetOptionsData(OptionsData optionsData)
    {
        _optionsData = optionsData;

        // Update the music volume based on both the slider and the modifier of the music scriptable object.

        if (_musicAudioSource != null)
        {
            Sound soundToPlay = SceneNav.IsGameplay() ? Sound.GameMusic : Sound.MenuMusic;
            _sounds.TryGetValue(soundToPlay, out SoundSO clip);

            if (clip == null) {
                Debug.LogError("The sound " + soundToPlay + " couldn't be found.");
                return;
            }

            _musicAudioSource.volume = _optionsData.masterVolume * _optionsData.musicVolume * clip.VolumeModifier;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        Configure();
    }

    private void Configure()
    {
        // Configure all the necessary data for the SoundManager to properly work.

        _soundData = Resources.Load<SoundData>(_soundsDataLoadPath);
        _optionsData = OptionsManager.LoadData(Application.persistentDataPath + "/bubble_gum_royale_player_data.json");

        if (_soundData == null) {
            Debug.LogError("The SoundData couldn't be properly loaded!!!");
        }

        if (_optionsData == null)
        {
            _optionsData = new OptionsData
            {
                language = LanguageType.English,
                musicVolume = 0.5f,
                masterVolume = 0.5f,
                sfxVolume = 0.5f
            };
        
            OptionsManager.SaveData(_optionsData, Application.persistentDataPath + "/bubble_gum_royale_player_data.json");
        }

        bool playMusic = false;
        if (transform.childCount == 0)
        {
            // This case might occur when we want to play a minigame directly from gameplay, for example.

            gameObject.AddComponent<AudioListener>();

            if (_soundData != null)
            {
                _defaultAudioSource = Instantiate(_soundData.defaultSfxAudioSource, transform).GetComponent<AudioSource>();
                _musicAudioSource = Instantiate(_soundData.defaultMusicAudioSource, transform).GetComponent<AudioSource>();

                playMusic = true;
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

        if (playMusic)
        {
            Sound soundToPlay = SceneNav.IsGameplay() ? Sound.GameMusic : Sound.MenuMusic;
            PlaySound(soundToPlay);
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

                if (!(lastTimePlayed + _soundData.playerMoveTimerMax < Time.time))
                    return false;
                    
                _soundTimerDictionary[sound] = Time.time;
                return true;
            }
        }
    }

    /// <summary>
    /// A direct and easy approach to playing any sound at any time.
    /// </summary>
    public void PlaySound(Sound sound)
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
            _musicAudioSource.volume = _optionsData.masterVolume * _optionsData.musicVolume * clip.VolumeModifier;
            _musicAudioSource.PlayDelayed(1f);
            return;
        }
        
        AudioClip clipToPlay =
              clip.OtherClips != null && clip.OtherClips.Count > 0
            ? clip.OtherClips[Random.Range(0, clip.OtherClips.Count)]
            : clip.Clip;


        float soundVolume = _optionsData.masterVolume * _optionsData.sfxVolume * clip.VolumeModifier;

        // If not a music, modify pitch and play the sound

        _defaultAudioSource.pitch = Random.Range(_soundData.defaultPitch - _soundData.defaultPitchModifier, _soundData.defaultPitch + _soundData.defaultPitchModifier);
        _defaultAudioSource.PlayOneShot(clipToPlay, soundVolume);
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
            source.volume = _optionsData.masterVolume * _optionsData.musicVolume * clip.VolumeModifier;
            source.PlayDelayed(1f);
            return;
        }

        float soundVolume = _optionsData.masterVolume * _optionsData.sfxVolume * clip.VolumeModifier;

        source.pitch = Random.Range(_soundData.defaultPitch - _soundData.defaultPitchModifier, _soundData.defaultPitch + _soundData.defaultPitchModifier);
        source.PlayOneShot(clip.Clip, soundVolume);
    }
}

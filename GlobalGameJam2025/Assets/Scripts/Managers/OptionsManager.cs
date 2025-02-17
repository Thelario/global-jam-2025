using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

[System.Serializable]
public enum LanguageType
{
    English = 0, Spanish = 1, French = 2
}

[System.Serializable]
public class OptionsData
{
    public LanguageType language;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
};

public class OptionsManager : Singleton<OptionsManager>
{
    [SerializeField] private TMP_Text languageText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private string _filePath;
    private OptionsData _currentLoadedOptions;

    protected override void Awake()
    {
        base.Awake();

        _filePath = Application.persistentDataPath + "/bubble_gum_royale_player_data.json";

        _currentLoadedOptions = LoadData(_filePath);
    }

    private void OnEnable()
    {
        LoadOptions();

        if (masterVolumeSlider != null) {
            masterVolumeSlider.onValueChanged.AddListener(OnMasterValueChanged);
        }

        if (musicVolumeSlider != null) {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicValueChanged);
        }

        if (sfxVolumeSlider != null) {
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxValueChanged);
        }

        if (leftButton != null) {
            leftButton.onClick.AddListener(OnLeftButtonPressed);
        }

        if (rightButton != null) {
            rightButton.onClick.AddListener(OnRightButtonPressed);
        }
    }

    private void OnDisable()
    {
        SaveOptions();

        masterVolumeSlider.onValueChanged.RemoveListener(OnMasterValueChanged);
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxValueChanged);
        leftButton.onClick.RemoveListener(OnLeftButtonPressed);
        rightButton.onClick.RemoveListener(OnRightButtonPressed);
    }

    private void LoadOptions()
    {
        _currentLoadedOptions = LoadData(_filePath);

        languageText.text = _currentLoadedOptions.language.ToString();

        if (masterVolumeSlider != null) {
            masterVolumeSlider.value = _currentLoadedOptions.masterVolume;
        }

        if (musicVolumeSlider != null) {
            musicVolumeSlider.value = _currentLoadedOptions.musicVolume;
        }

        if (sfxVolumeSlider != null) {
            sfxVolumeSlider.value = _currentLoadedOptions.sfxVolume;
        }
    }

    private void SaveOptions()
    {
        SaveData(_currentLoadedOptions, _filePath);
    }

    public static void SaveData(OptionsData data, string filePath)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public static OptionsData LoadData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<OptionsData>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }

    #region On Value Changed

    private void OnRightButtonPressed()
    {
        int next = (int)_currentLoadedOptions.language + 1;
        if (next >= Enum.GetValues(typeof(LanguageType)).Length)
            next = 0;

        _currentLoadedOptions.language = (LanguageType)next;
        languageText.text = _currentLoadedOptions.language.ToString();
    }

    private void OnLeftButtonPressed()
    {
        int next = (int)_currentLoadedOptions.language -1;
        if (next < 0)
            next = Enum.GetValues(typeof(LanguageType)).Length - 1;

        _currentLoadedOptions.language = (LanguageType)next;
        languageText.text = _currentLoadedOptions.language.ToString();
    }

    private void OnMasterValueChanged(float value)
    {
        _currentLoadedOptions.masterVolume = value;
        SoundManager.Instance.SetOptionsData(_currentLoadedOptions);
    }

    private void OnMusicValueChanged(float value)
    {
        _currentLoadedOptions.musicVolume = value;
        SoundManager.Instance.SetOptionsData(_currentLoadedOptions);
    }

    private void OnSfxValueChanged(float value)
    {
        _currentLoadedOptions.sfxVolume = value;
        SoundManager.Instance.SetOptionsData(_currentLoadedOptions);
    }

    #endregion
}
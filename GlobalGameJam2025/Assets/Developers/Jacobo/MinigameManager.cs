using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class MinigameManager : Singleton<MinigameManager>
{
    // Events
    public event UnityAction OnMinigameInit;
    public event UnityAction OnMinigameStart;
    public event UnityAction OnMinigameEnd;

    // Properties
    public MinigameBase CurrentMinigame { get; private set; }
    public int GameRounds { get; private set; } = 4;
    public int GameTimer { get; private set; } = 60;

    // Private Fields
    private static List<MinigameBase> m_GameList = new List<MinigameBase>();
    private List<MultiplayerInstance> playersDead = new List<MultiplayerInstance>();
    private List<MultiplayerInstance> allPlayers = new List<MultiplayerInstance>();
    private float currentTimer = 0f;
    private bool timerOn = false;

    [SerializeField] private MinigameBase TESTING_GAME;

    #region Unity Lifecycle

    private void OnEnable() => SceneManager.sceneLoaded += InitMinigameOnLoad;

    private void OnDisable() => SceneManager.sceneLoaded -= InitMinigameOnLoad;

    private void Start()
    {
        if (TESTING_GAME != null)
        {
            InitMinigameInfo(1, 20);
            CurrentMinigame = TESTING_GAME;
            InitMinigame();
        }
    }

    private void Update()
    {
        CurrentMinigame?.MinigameUpdate();

        if (timerOn)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                timerOn = false;
                EndMinigame();
            }
        }
    }

    #endregion

    #region Minigame Initialization and Management

    public void InitMinigameInfo(int rounds, int timer, int gameIndex = 0)
    {
        GameRounds = rounds;
        GameTimer = timer;
        if (gameIndex == 0) AssignMinigamesRandom();
        else AssignMinigame(gameIndex);
    }

    public void InitMinigame()
    {
        if (!SceneNav.IsGameplay() || m_GameList.Count == 0 || CurrentMinigame != null) return;

        CurrentMinigame = m_GameList[0];
        m_GameList.RemoveAt(0);

        OnMinigameInit?.Invoke();
        CurrentMinigame.MinigameInit();

        if (AssetLocator.MainCanvasPrefab) Instantiate(AssetLocator.MainCanvasPrefab);
        SpawnPlayers();

        StartMinigame();
    }

    public void StartMinigame()
    {
        playersDead.Clear();

        if (CurrentMinigame != null)
        {
            OnMinigameStart?.Invoke();
            CurrentMinigame.MinigameStart();
        }

        StartTimer();
    }

    public void EndMinigame()
    {
        if (CurrentMinigame == null) return;

        OnMinigameEnd?.Invoke();
        CurrentMinigame.MinigameEnd();
        CurrentMinigame = null;

        AddRemainingPlayersToScore();
        SceneNav.GoTo(SceneType.Score);
    }

    private void StartTimer()
    {
        currentTimer = 5;// GameTimer;
        timerOn = true;
    }

    #endregion

    #region Minigame Assignment

    private void AssignMinigamesRandom()
    {
        if (AssetLocator.ALLGAMES == null || AssetLocator.ALLGAMES.Count < 4) return;

        var availableMinigames = new List<MinigameBase>(AssetLocator.ALLGAMES);
        m_GameList = new List<MinigameBase>();

        for (int i = 0; i < GameRounds; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableMinigames.Count);
            m_GameList.Add(availableMinigames[randomIndex]);
            availableMinigames.RemoveAt(randomIndex);
        }
    }

    private void AssignMinigame(int index)
    {
        if (AssetLocator.ALLGAMES == null || AssetLocator.ALLGAMES.Count < 4) return;

        var availableMinigames = new List<MinigameBase>(AssetLocator.ALLGAMES);
        m_GameList = new List<MinigameBase>();

        for (int i = 0; i < GameRounds; i++)
            m_GameList.Add(availableMinigames[index]);
    }

    #endregion

    #region Player Management

    public List<MultiplayerInstance> GetAllPlayers() => allPlayers;

    public List<MultiplayerInstance> GetLastGameScore() => playersDead;

    public void PlayerDeath(MultiplayerInstance player)
    {
        playersDead.Add(player);
        if (playersDead.Count >= GameManager.Instance.NumberOfPlayers() - 1) EndMinigame();
    }

    private void AddRemainingPlayersToScore()
    {
        foreach (var player in allPlayers)
        {
            if (!playersDead.Contains(player))
                playersDead.Add(player);
        }
    }

    public void SpawnPlayers()
    {
        allPlayers.Clear();
        var spawnPoints = SpawnPoint.GetSpawnPoints();
        var playerDataList = GameManager.Instance.GetAllPlayer();

        EnsureEnoughSpawnPoints(spawnPoints, playerDataList.Count);

        for (int i = 0; i < playerDataList.Count; i++)
        {
            var playerObject = Instantiate(AssetLocator.PlayerPrefab(), spawnPoints[i], Quaternion.identity);
            if (playerObject.TryGetComponent(out MultiplayerInstance multiplayerInstance))
            {
                multiplayerInstance.AssignData(playerDataList[i]);
                allPlayers.Add(multiplayerInstance);
            }
        }
    }

    private void EnsureEnoughSpawnPoints(List<Vector3> spawnPoints, int requiredCount)
    {
        while (spawnPoints.Count < requiredCount)
            spawnPoints.Add(spawnPoints[spawnPoints.Count - 1]);
    }

    #endregion

    #region Scene Loading

    private void InitMinigameOnLoad(Scene scene, LoadSceneMode mode)
    {
        InitMinigame();
    }

    #endregion
}

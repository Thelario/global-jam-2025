using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class MinigameManager : Singleton<MinigameManager>
{
    // Events
    public event UnityAction OnMinigameInit;
    public event UnityAction OnMinigameStart;
    public event UnityAction OnMinigameEnd;

    // Properties
    public MinigameBase CurrentMinigame { get; private set; }
    public int GameRounds { get; private set; } = 5;
    public int RoundsLeft()
    {
        if (m_GameList != null) return m_GameList.Count();
        else return 0;
    }
    public int GameMaxTimer { get; private set; } = 60;
    public float GameTimer
    {
        get { return currentTimer; }
        private set { currentTimer = Mathf.Max(0, value); }
    }

    // Private Fields
    private static List<MinigameBase> m_GameList = new List<MinigameBase>();
    private List<PlayerData> playersDead = new List<PlayerData>();
    private List<MultiplayerInstance> allPlayers = new List<MultiplayerInstance>();
    private float currentTimer = 0f;
    private bool timerOn = false;

    #region Unity Lifecycle

    private void OnEnable() => SceneManager.sceneLoaded += InitMinigameOnLoad;

    private void OnDisable() => SceneManager.sceneLoaded -= InitMinigameOnLoad;

    private void Start()
    {
        //Testin only
        if (SceneNav.IsGameplay())
        {
            InitMinigameInfo(1, 30);
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
        GameMaxTimer = timer;
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

        //StartMinigame();
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
    }

    private void StartTimer()
    {
        currentTimer = GameMaxTimer;
        timerOn = true;
    }

    #endregion

    #region Minigame Assignment

    private void AssignMinigamesRandom()
    {
        if (AssetLocator.ALLGAMES == null || AssetLocator.ALLGAMES.Count < 4) return;

        var availableMinigames = new List<MinigameBase>(AssetLocator.ALLGAMES);
        m_GameList = new List<MinigameBase>();

        //for (int i = 0; i < GameRounds; i++)
        for (int i = 0; i < 100; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableMinigames.Count);
            m_GameList.Add(availableMinigames[randomIndex]);
            //availableMinigames.RemoveAt(randomIndex);
        }
    }

    private void AssignMinigame(int index)
    {
        if (AssetLocator.ALLGAMES == null || AssetLocator.ALLGAMES.Count < 4) return;

        var availableMinigames = new List<MinigameBase>(AssetLocator.ALLGAMES);
        m_GameList = new List<MinigameBase>();

        for (int i = 0; i < 1000; i++)
            m_GameList.Add(availableMinigames[index]);
    }

    #endregion

    #region Player Management

    public List<MultiplayerInstance> GetAllPlayers() => allPlayers;

    public List<PlayerData> GetLastGameScore() => playersDead;

    public void PlayerDeath(MultiplayerInstance player)
    {
        playersDead.Add(player.PlayerData);
        int numberPlayers = GameManager.Instance.NumberOfPlayers();
        if (playersDead.Count >= numberPlayers - 1) EndMinigame();
    }

    private void AddRemainingPlayersToScore()
    {
        foreach (var player in allPlayers)
        {
            if (!playersDead.Contains(player.PlayerData))
                playersDead.Add(player.PlayerData);
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
                multiplayerInstance.GetComponent<PlayerController>().EnableRollVolume(true);
                multiplayerInstance.AssignData(playerDataList[i]);
                allPlayers.Add(multiplayerInstance);
            }
        }

        //ShowCrown();
    }

    void ShowCrown()
    {
        PlayerData winningPlayer = null;
        int topPoints = 0;

        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (allPlayers[i].PlayerData.TotalPoints > topPoints)
            {
                winningPlayer = allPlayers[i].PlayerData;
                topPoints = winningPlayer.TotalPoints;
            }
        }

        MultiplayerInstance winningPlayerInstance = null;
        MultiplayerInstance[] multiplayerInstances = FindObjectsByType<MultiplayerInstance>(FindObjectsSortMode.None);
        for (int i = 0; i < multiplayerInstances.Length; i++)
        {
            if (multiplayerInstances[i].playerIndex == winningPlayer.Index)
            {
                winningPlayerInstance = multiplayerInstances[i];
                Crown.instance.playerFollow = winningPlayerInstance.transform;
            }
        }
    }

    public void SpawnPlayerByIndex(int playerIndex, Vector3 playerPosition)
    {
        var playerDataList = GameManager.Instance.GetAllPlayer();

        var playerObject = Instantiate(AssetLocator.PlayerPrefab(), playerPosition, Quaternion.identity);
        if (playerObject.TryGetComponent(out MultiplayerInstance multiplayerInstance))
        {
            multiplayerInstance.GetComponent<PlayerController>().EnableRollVolume(true);
            multiplayerInstance.AssignData(playerDataList[playerIndex]);
            allPlayers.Add(multiplayerInstance);
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

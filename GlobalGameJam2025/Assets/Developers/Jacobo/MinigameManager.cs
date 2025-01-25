using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Collections;

/// <summary>
/// MINIGAME MANAGER: Al comienzo, busca 
/// </summary>
public class MinigameManager : Singleton<MinigameManager>
{
    // Events
    public event UnityAction OnMinigameInit;
    public event UnityAction OnMinigameStart;
    public event UnityAction OnMinigameEnd;

    private static List<MinigameBase> m_GameList = new List<MinigameBase>();//Todos los juegos que se van a jugar
    private MinigameBase m_currentMinigame;
    public MinigameBase CurrentMinigame => m_currentMinigame;
    
    private int m_GameRounds = 4;
    public int GameRounds => m_GameRounds;

    private int m_GameTimer = 60;
    public int GameTimer => m_GameTimer;

    #region Assign Minigame
    //Diccionario cargado por Lazy, carga todos los minijuegos en Resources
    private static Lazy<Dictionary<MinigameBase, string>> m_GamesLoaded = new Lazy<Dictionary<MinigameBase, string>>(() =>
    {
        var gameList = new Dictionary<MinigameBase, string>();
        var minigameFiles = Resources.LoadAll<MinigameBase>("");
        foreach (var minigame in minigameFiles) gameList.Add(minigame, minigame.Name());
        return gameList;
    });
    public static Dictionary<MinigameBase, string> GamesLoaded() => m_GamesLoaded.Value;

    
    [SerializeField] private MinigameBase TESTING_GAME;//TODO:SOLO TESTEO

    public void InitMinigameInfo(int rounds, int timer, int gameIndex = 0)
    {
        m_GameRounds = rounds;
        m_GameTimer = timer;
        if (gameIndex == 0) AssignMinigamesRandom();
        else AssignMinigame(gameIndex);
        Debug.Log($"Rounds:{m_GameRounds}, Timer:{m_GameTimer}, Random:{m_GameList}");
    }

    public void AssignMinigamesRandom()//Assign Random Levels
    {
        if (m_GamesLoaded.Value.Count < 4) return;
        
        var availableMinigames = new List<MinigameBase>(m_GamesLoaded.Value.Keys);
        for (int i = m_GameRounds; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            var temp = availableMinigames[i];
            availableMinigames[i] = availableMinigames[randomIndex];
        }
        m_GameList = availableMinigames;
    }
    public void AssignMinigame(int index)//Assign specific minigame
    {
        if (m_GamesLoaded.Value.Count < 4) return;

        m_GameList = new List<MinigameBase>();
        var availableMinigames = new List<MinigameBase>(m_GamesLoaded.Value.Keys);
        for (int i = m_GameRounds; i > 0; i--)
        {
            m_GameList.Add(availableMinigames[index]);
        }
    }
    #endregion
    private void OnEnable()
    {
        SceneManager.sceneLoaded += InitMinigameOnLoad;
    }

    private void OnDisable() => SceneManager.sceneLoaded -= InitMinigameOnLoad;
    private void InitMinigameOnLoad(Scene scene, LoadSceneMode mode)
    {
        InitMinigame();
    }

    private void Start()
    {
        if (TESTING_GAME != null)
        {
            InitMinigameInfo(1, 20);
            m_currentMinigame = TESTING_GAME;
            InitMinigame();
        }
    }
    public void InitMinigame()
    {
        if (m_GameList.Count == 0) return;
        if (m_currentMinigame) EndMinigame();

        //Terminar el current, asignar y quitarlo de la lista. Inicializarlo y empezar
        m_currentMinigame = m_GameList[0];
        m_GameList.RemoveAt(0);

        OnMinigameInit?.Invoke();
        m_currentMinigame.MinigameInit();
        
        if (AssetLocator.MainCanvasPrefab) Instantiate(AssetLocator.MainCanvasPrefab);
        SpawnPlayers();
        
        StartMinigame();
    }

    public void StartMinigame()
    {
        playersDead = new List<MultiplayerInstance>();//Clear Dead Players
        if (m_currentMinigame)
        {
            OnMinigameStart?.Invoke();
            m_currentMinigame.MinigameStart();
        }
        StartTimer();
    }
    private bool timerOn = false;
    private float currentTimer = 0f;
    private void StartTimer()
    {
        currentTimer = m_GameTimer;
        timerOn = true;
    }

    private void Update()
    {
        if (m_currentMinigame) m_currentMinigame.MinigameUpdate();
        
        //Timer
        if (timerOn)
        {
            Debug.Log($"TIMER: {currentTimer}");
            currentTimer -= Time.deltaTime;
            if(currentTimer < 0f)
            {
                timerOn = false;
                EndMinigame();
            }
        }
    }
    private List<MultiplayerInstance> playersDead;

    //Primero es el que primero ha muerto
    public List<MultiplayerInstance> GetLastGameScore() => playersDead;
    public void PlayerDeath(MultiplayerInstance data)
    {
        playersDead.Add(data);
        if (playersDead.Count >= 3) EndMinigame();
    }
    public void EndMinigame()
    {
        if (!m_currentMinigame) return;

        OnMinigameEnd?.Invoke();
        m_currentMinigame.MinigameEnd();

        m_currentMinigame = null;

        foreach(MultiplayerInstance pl in allPlayers)
        {
            if (playersDead.Contains(pl)) continue;
            playersDead.Add(pl);
        }
        SceneNav.GoTo(SceneType.Score);
    }
    private GameObject playerPrefab;
    private List<MultiplayerInstance> allPlayers;
    public List<MultiplayerInstance> GetAllPlayers() => allPlayers;
    
    public void SpawnPlayers()
    {
        playerPrefab = AssetLocator.PlayerPrefab();
        allPlayers = new List<MultiplayerInstance>();

        //Add keyboard si no hay ninguno conectado (para testeo)
        if(GameManager.Instance.GetAllPlayer().Count == 0) 
            GameManager.Instance.AddPlayer(Keyboard.current);
        
        //Spawn Positions
        List<Vector3> newPosList = SpawnPoint.GetSpawnPoints();
        List<PlayerData> allPlayerData = GameManager.Instance.GetAllPlayer();
        
        //Asegurarse que hay suficientes spawnpoints, o crear mas
        if (newPosList.Count < allPlayerData.Count)
        {
            int missingSpawnPoints = allPlayerData.Count - newPosList.Count;
            Vector3 lastSpawnPoint = newPosList[newPosList.Count - 1];
            for (int i = 0; i < missingSpawnPoints; i++) newPosList.Add(lastSpawnPoint);
        }
        
        for (int i = 0; i < allPlayerData.Count; i++)
        {
            GameObject gb = Instantiate(playerPrefab, newPosList[i], Quaternion.identity);
            if (gb.TryGetComponent(out MultiplayerInstance multi))
            {
                multi.AssignData(allPlayerData[i]);
                allPlayers.Add(multi);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// MINIGAME MANAGER: Al comienzo, busca 
/// </summary>
public class MinigameManager : Singleton<MinigameManager>
{
    // Events
    public event UnityAction OnMinigameInit;
    public event UnityAction OnMinigameStart;
    public event UnityAction OnMinigameEnd;

    private static List<MinigameBase> m_GameList;//Todos los juegos que se van a jugar
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

    protected override void Awake()
    {
        base.Awake();
        //TODO: Remove on build
        if (TESTING_GAME) m_GameList = new List<MinigameBase>() { TESTING_GAME };
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        InitMinigame();
    }
    public void InitMinigame()
    {
        if (m_GameList == null || m_GameList.Count == 0|| m_GameList == null) return;
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
        if (m_currentMinigame)
        {
            OnMinigameStart?.Invoke();
            m_currentMinigame.MinigameStart();
        }
    }

    private void Update()
    {
        if (m_currentMinigame) m_currentMinigame.MinigameUpdate();
    }

    public void EndMinigame()
    {
        if (m_currentMinigame)
        {
            OnMinigameEnd?.Invoke();
            m_currentMinigame.MinigameEnd();
            Debug.Log("Minigame Ended: " + m_currentMinigame.name);
            m_currentMinigame = null;
        }
    }
    private GameObject playerPrefab;
    private List<MultiplayerInstance> allPlayers;
    public List<MultiplayerInstance> GetAllPlayers() => allPlayers;
    
    public void SpawnPlayers()
    {
        playerPrefab = AssetLocator.PlayerPrefab();
        allPlayers = new List<MultiplayerInstance>();

        //if(GameManager.Instance.GetAllPlayer().Count == 0) GameManager.Instance.AddPlayer(Keyboard.current);
        //List<Vector3> newPosList = new List<Vector3>();
        //List<SpawnPoint> spawnPoints = SpawnPoint.GetSpawnPoints();
        //foreach (var pos in spawnPoints)
        //{
        //    newPosList.Add(pos.SpawnPosition);
        //}

        List<PlayerData> allPlayerData = GameManager.Instance.GetAllPlayer();
        for (int i = 0; i < allPlayerData.Count; i++)
        {
            GameObject gb = Instantiate(playerPrefab, Vector3.up * 2 *i, Quaternion.identity);
            if (gb.TryGetComponent(out MultiplayerInstance multi)) allPlayers.Add(multi);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// MINIGAMEMANAGER ahora solo se encarga de controlar un solo minijuego por vez.
/// Solo existe en la escena de Gameplay. Obtiene la referencia del GameData de GameManager
/// Y coordina todos los otros sistemas del Minijuego
/// </summary>

public class MinigameManager : Singleton<MinigameManager>
{
    //Init se llama OnStart, se encarga de que se instancie todo correctamente
    //Desde jugadores a Prefabs de jugadores, etc.

    //Start se llama cuando comienza el minijuego; puede ser util para algunos obstaculos
    //para empezar a girar, moverse, etc.

    //End se llama una vez termina el juego por cualquier motivo. El prefab del minijuego 
    //ya no deberia destruirse aqui

    //public event UnityAction OnMinigameInit;
    //public event UnityAction OnMinigameStart;
    //public event UnityAction OnMinigameEnd;
    
    private List<IMinigameEventListener> eventListeners = new List<IMinigameEventListener>();
    
    //Cuando muere un personaje. Puede servir para mas obstaculos, hacer el nivel mas pequeno, etc.
    public event UnityAction OnPlayerDeath;

    //Lista de jugadores spawneados esta ronda
    public List<PlayerCore> PlayerList { get; private set; } = new List<PlayerCore>();

    //Timer desde el comienzo del minijuego, no solo para terminar juego
    public float Timer { get; private set; } = 0f;
    private bool shouldCountTimer = false;

    //Referencias
    GameManager gameManager;
    GameData currentGameData;
    MinigameData currentMinigame;

    //PARA TESTING
    public MinigameData TestingGame;

    #region Init / Organizacion
    
    private void Start()
    {
        //Inicializa la lista de objetos que implementan IMinigameEventListener.
        //Cada vez que ocurra un evento (Game Start, Player Death, et.) se les notificara
        RegisterAllListeners();
        //Asigna el juego, ya sea desde GameManager, o Test desde inspector
        AssignGame();
        //Spawnear Prefab del minijuego
        Instantiate(currentMinigame.MiniGamePrefab);
        //Spawnear Jugadores en Spawnpoints
        SpawnPlayers();
        //Cuando se desconecte un mando, matar jugador
        if(gameManager) gameManager.OnPlayerRemoved += KillPlayer;
        //Inicializar sistemas. En este punto ya esta todo listo, comienza cuenta atras
        NotifyMinigameInit();
    }
    
    private void AssignGame()
    {
        gameManager = GameManager.Instance;
        if (gameManager.CurrentGame != null)
        {
            //Obtiene minijuego y lo elimina de la lista
            currentGameData = gameManager.CurrentGame;
            currentMinigame = currentGameData.GetNextMinigame();
        }
        else if (TestingGame != null)
        {
            //Crea un nuevo GameData con el minijuego test del inspector
            GameData testGameData = new GameData(new List<MinigameData>() { TestingGame }, 10);
            gameManager.CreateGameData(testGameData,
                gameManager.PlayerConnection.GetAllConnectedDevices());

            currentGameData = gameManager.CurrentGame;
            currentMinigame = currentGameData.GetNextMinigame();
        }
        else
        {
            Debug.Log("Mingame not initialised in GameManager");
            SceneNav.GoTo(SceneType.PlayerSelect);
            return;
        }
    }

    private void OnDestroy()
    {
        if(gameManager) gameManager.OnPlayerRemoved -= KillPlayer;
    }
    private void SpawnPlayers()
    {
        PlayerCore playerPrefab = AssetLocator.Data.PlayerPrefab;
        if (!playerPrefab) return;
        List<Vector3> spawnPositions = SpawnPoint.GetSpawnPoints();
        
        //En el caso de que no haya suficientes spawnpoints en el prefab, 
        //instanciar todo en 0 0 0
        if(gameManager.PlayerCount > spawnPositions.Count)
        {
            Debug.Log("Faltan spawnpoints por colocar!" + spawnPositions.Count);
            spawnPositions = new List<Vector3>(gameManager.PlayerCount) { new Vector3(0,1,0)};
        }
        
        //Meter todos los jugadores en un Empty, instanciarlos e inicializarlos
        GameObject playerHolder = new GameObject("Player Holder");
        playerHolder.transform.Reset();
        List<PlayerData> dataList = currentGameData.GamePlayers;
        for (int i = 0; i < dataList.Count; i++)
        {
            PlayerCore playerIns = Instantiate(playerPrefab, spawnPositions[i], Quaternion.identity, playerHolder.transform);
            playerIns.InitPlayer(dataList[i]);
            PlayerList.Add(playerIns);
        }
        
    }
    #endregion

    #region Minigame Methods
    
    //Despues de cuenta atras
    public void StartMinigame()
    {
        Timer = GameSettings.MAX_TIMER;
        shouldCountTimer = true;
        foreach (var pl in PlayerList)
        {
            CreatePointsVisualizer(pl.transform.position);
            pl.ToggleMovement(true);
        }

        NotifyMinigameStart();
    }

    private void CreatePointsVisualizer(Vector3 playerPos)
    {
        PlayerPoints pts = Instantiate(AssetLocator.Data.PlayerPointsPrefab,
            playerPos, Quaternion.identity);
        pts.Init(2);
    }

    public void EndMinigame()
    {
        NotifyMinigameEnd();
        SceneNav.GoTo(SceneType.PlayerSelect);
    }

    public void PlayerDeath(PlayerCore player)
    {
        if (!PlayerList.Contains(player)) return;
        CreatePointsVisualizer(player.transform.position);
        PlayerList.Remove(player);
        
        player.KillPlayer();
        NotifyPlayerDeath(player);

        if (PlayerList.Count <= 1) EndMinigame();
    }

    //Se utiliza cuando se desconecta un mando, para matar al jugador.
    public void KillPlayer(PlayerData arg0)
    {
        List<PlayerCore> copyList = PlayerList;
        foreach(var player in PlayerList) 
        {
            if (player != null && player.PlayerData == arg0) 
            {
                PlayerDeath(player);
                return;
            }
        }
    }

    private void Update()
    {
        if (shouldCountTimer)
        {
            if(Timer > 0) Timer -= Time.deltaTime;
            else
            {
                Timer = 0;
                shouldCountTimer = false;
                EndMinigame();
            }
        }
    }

    #endregion

    #region EVENTS
    private void RegisterAllListeners()
    {
        // Find all objects of type IMinigameEventListener in the scene
        var listeners = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IMinigameEventListener>();

        foreach (var listener in listeners)
        {
            if (listener is IMinigameEventListener eventListener)
            {
                eventListeners.Add(eventListener);
            }
        }
    }
    private void NotifyMinigameInit() { foreach (var listener in eventListeners) listener.OnMinigameInit(); }
    private void NotifyMinigameStart() { foreach (var listener in eventListeners) listener.OnMinigameStart(); }
    private void NotifyMinigameEnd() { foreach (var listener in eventListeners) listener.OnMinigameEnd(); }
    private void NotifyPlayerDeath(PlayerCore player) { foreach (var listener in eventListeners) listener.OnPlayerDeath(player); }
    #endregion
}

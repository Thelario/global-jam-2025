using System;
using System.Collections.Generic;
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
    public event UnityAction OnMinigameInit;
    public event UnityAction OnMinigameStart;
    public event UnityAction OnMinigameEnd;

    //Lista de jugadores spawneados esta ronda
    public List<PlayerCore> PlayerList { get; private set; }

    //Timer desde el comienzo del minijuego, no solo para terminar juego
    public float Timer { get; private set; } = 0f;
    private bool shouldCountTimer = false;

    //Referencias
    GameManager gameManager;
    GameData currentGameData;
    MinigameData currentMinigame;
   
    #region Init / Organizacion

    private void Start()
    {
        PlayerList = new List<PlayerCore>();

        //Obtiene la referencia del minijuego a jugar, y lo elimina de la lista
        gameManager = GameManager.Instance;
        if (gameManager.CurrentGame != null)
        {
            currentGameData = gameManager.CurrentGame;
            currentMinigame = currentGameData.GetNextMinigame();
        }
        else
        {
            Debug.Log("Mingame not initialised in GameManager");
            return;
        }
        if (!currentMinigame) return;

        //Spawnear Prefab del minijuego
        Instantiate(currentMinigame.MiniGamePrefab);
        //Spawnear Jugadores en Spawnpoints
        SpawnPlayers();
        //Cuando se desconecte un mando, matar jugador
        if(gameManager) gameManager.OnPlayerRemoved += KillPlayer;
        //Inicializar sistemas. En este punto ya esta todo listo, comienza cuenta atras
        OnMinigameInit?.Invoke();

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
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
        OnMinigameStart?.Invoke();
        Timer = GameSettings.MAX_TIMER;
        shouldCountTimer = true;
        foreach (var pl in PlayerList)
        {
            pl.ToggleMovement(true);
            GameplayUI.Instance.ShowPlayerPoints(pl);
        }
    }
    public void EndMinigame()
    {
        OnMinigameEnd?.Invoke();
    }

    public void KillPlayer(PlayerCore player)
    {
        PlayerList.Remove(player);
        Destroy(player.gameObject);

        if (PlayerList.Count <= 1) SceneNav.GoTo(SceneType.PlayerSelect);
    }

    //Se utiliza cuando se desconecta un mando, para matar al jugador.
    public void KillPlayer(PlayerData arg0)
    {
        List<PlayerCore> copyList = PlayerList;
        foreach(var player in PlayerList) 
        {
            if (player != null && player.PlayerData == arg0) 
            {
                KillPlayer(player);
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
}

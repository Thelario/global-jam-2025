using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// MINIGAMEMANAGER ahora solo se encarga de controlar un solo minijuego por vez.
/// Solo existe en la escena de Gameplay. Obtiene la referencia del GameData de GameManager
/// Y coordina todos los otros sistemas del Minijuego
/// </summary>

public class NewMinigameManager : Singleton<NewMinigameManager>
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

    //Referencias
    GameManager gameManager;
    GameData currentGameData;
    MinigameData currentMinigame;
   
    #region Init / Organizacion
    protected override void Awake()
    {
        //Init Everything
        base.Awake();
        PlayerList = new List<PlayerCore>();

        //Obtiene la referencia del minijuego a jugar, y lo elimina de la lista
        gameManager = GameManager.Instance;
        currentGameData = gameManager.CurrentGame;
        currentMinigame = currentGameData.GetNextMinigame();
    }
    private void Start()
    {
        //Spawnear Prefab del minijuego
        Instantiate(currentMinigame.MiniGamePrefab);
        //Spawnear Jugadores en Spawnpoints
        SpawnPlayers();
        //Cuando se desconecte un mando, matar jugador
        gameManager.OnPlayerRemoved += KillPlayer;
        //Inicializar sistemas. En este punto ya esta todo listo, comienza cuenta atras
        OnMinigameInit?.Invoke();
    }
    private void OnDestroy()
    {
        gameManager.OnPlayerRemoved -= KillPlayer;
    }
    private void SpawnPlayers()
    {
        PlayerCore playerPrefab = AssetLocator.PlayerPrefab;
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
        List<PlayerData> dataList = gameManager.GetPlayerList();
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
    }

    private void KillPlayer(PlayerCore player)
    {
        PlayerList.Remove(player);
        Destroy(player.gameObject);

        if (PlayerList.Count <= 1) SceneNav.GoTo(SceneType.PlayerSelect);
    }

    //Se utiliza cuando se desconecta un mando, para matar al jugador.
    private void KillPlayer(PlayerData arg0)
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
 
    #endregion
}

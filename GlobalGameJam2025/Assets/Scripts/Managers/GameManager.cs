using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : PersistentSingleton<GameManager>
{
    public PlayerConnection PlayerConnection { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        PlayerConnection = gameObject.AddComponent<PlayerConnection>();
        PlayerConnection.Init();
    }

    #region PLAYER MANAGEMENT

    //Esta lista es de Jugadores CONECTADOS, NO JUGANDO (se puede
    //conectar/desconectar mandos durante la partida). Sino, utilizar la lista de Jugadores
    //dentro del GameData de la partida
    private List<PlayerData> m_AllPlayersConnected = new List<PlayerData>();
    public List<PlayerData> GetPlayerList() => m_AllPlayersConnected;
    public int GetPlayerIndex(PlayerData data)
    {
        if (data == null) return -1;
        return m_AllPlayersConnected.IndexOf(data);
    }
    public void SetPlayerData(int playerIndex, PlayerData newPlayerData)
    {
        if (m_AllPlayersConnected[playerIndex] != null)
        {
            m_AllPlayersConnected[playerIndex] = newPlayerData;
        }
    }
    public int PlayerCount => m_AllPlayersConnected.Count;

    // Events
    public event UnityAction<PlayerData> OnPlayerAdded;
    public event UnityAction<PlayerData> OnPlayerRemoved;

    //Logica manejada por PlayerConnection
    public void AddPlayer(PlayerData playerData)
    {
        if (playerData == null) return;

        m_AllPlayersConnected.Add(playerData);
        OnPlayerAdded?.Invoke(playerData);
    }

    public void RemovePlayer(PlayerData playerData)
    {
        if (playerData == null) return;

        m_AllPlayersConnected.Remove(playerData);
        OnPlayerRemoved?.Invoke(playerData);
    }
    public void RemovePlayer(int index)
    {
        if (m_AllPlayersConnected[index] != null)
        {
            RemovePlayer(m_AllPlayersConnected[index]);
        }
    }
    public void ClearAllPlayers()
    {
        m_AllPlayersConnected.Clear();
        PlayerConnection.Init();
    }

    #endregion

    #region GAME DATA MANAGEMENT

    public GameData CurrentGame { get; private set; }
    public void CreateGameData(GameData newGameData)
    {
        if (newGameData == null) return;
        newGameData.SetPlayers(m_AllPlayersConnected);
        CurrentGame = newGameData;
    }
    //Override que fuerza los jugadores al del parametro
    public void CreateGameData(GameData newGameData, List<PlayerData> allPlayers)
    {
        if (newGameData == null) return;
        newGameData.SetPlayers(allPlayers);
        CurrentGame = newGameData;
    }

    #endregion
}

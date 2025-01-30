using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    public void AssignGameData(GameData gameData)
    {
        if (gameData != null) CurrentGame = gameData;
    }

    #endregion
}

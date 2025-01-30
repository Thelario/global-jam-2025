using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : PersistentSingleton<GameManager>
{
    PlayerConnection playerConnect;
    protected override void Awake()
    {
        base.Awake();
        playerConnect = gameObject.AddComponent<PlayerConnection>();
        playerConnect.Init();
    }

    #region PLAYER MANAGEMENT

    private List<PlayerData> m_playerList = new List<PlayerData>();
    public List<PlayerData> GetPlayerList() => m_playerList;
    public int GetPlayerIndex(PlayerData data)
    {
        if (data == null) return -1;
        return m_playerList.IndexOf(data);
    }
    public void SetPlayerData(int playerIndex, PlayerData newPlayerData)
    {
        if (m_playerList[playerIndex] != null)
        {
            m_playerList[playerIndex] = newPlayerData;
        }
    }
    public int PlayerCount => m_playerList.Count;

    // Events
    public event UnityAction<PlayerData> OnPlayerAdded;
    public event UnityAction<PlayerData> OnPlayerRemoved;

    //Logica manejada por PlayerConnection
    public void AddPlayer(PlayerData playerData)
    {
        if (playerData == null) return;

        m_playerList.Add(playerData);
        OnPlayerAdded?.Invoke(playerData);
    }

    public void RemovePlayer(PlayerData playerData)
    {
        if (playerData == null) return;

        m_playerList.Remove(playerData);
        OnPlayerRemoved?.Invoke(playerData);
    }
    public void RemovePlayer(int index)
    {
        if (m_playerList[index] != null)
        {
            RemovePlayer(m_playerList[index]);
        }
    }
    public void ClearAllPlayers()
    {
        m_playerList.Clear();
        playerConnect.Init();
    }
    #endregion

    #region GAME DATA MANAGEMENT
    
    public GameData CurrentGame { get; private set; }
    internal void AssignGameData(GameData gameData)
    {
        if (gameData != null)
        {
            CurrentGame = gameData;
            Debug.Log("ASSINGED SUCCESFULLY: " + gameData.UpcomingMinigames.Count + " games");
        }
    }

    #endregion
}

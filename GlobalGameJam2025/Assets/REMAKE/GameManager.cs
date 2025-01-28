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
    }
    
    #region PLAYER MANAGEMENT

    private List<PlayerData> m_playerList = new List<PlayerData>();
    public List<PlayerData> GetPlayerList() => m_playerList;
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

    #endregion
}

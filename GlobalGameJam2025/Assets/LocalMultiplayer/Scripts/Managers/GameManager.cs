using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

/// <summary>
/// PlayerData se encarga de llevar la info de cada jugador
/// conectado. El input que usa y el numero de puntos que tiene.
/// Se deberia crear cuando se conecte un jugador, y se mantiene hasta
/// que se termine la partida y gane alguien
/// </summary>
[System.Serializable]
public enum SkinType
{
    Default = 0,
    Red = 1,
    Blue = 2,
    Green = 3
}
public class PlayerData
{
    public PlayerData(int newIndex, InputDevice newDevice, SkinType newSkinType = SkinType.Default)
    {
        this.m_index = newIndex;
        this.m_device = newDevice;
        this.m_SkinType = newSkinType;
        this.m_totalPoints = 0;
    }
    private int m_index;
    public int Index => m_index;

    private SkinType m_SkinType;
    private SkinType SkinType => m_SkinType;
    //Getters
    private InputDevice m_device;
    public InputDevice Device => m_device;
    
    private int m_totalPoints;
    public int TotalPoints => m_totalPoints;
    public void SetTotalPoints(int value)
    {
        m_totalPoints = value;
    }
}

public class GameManager : Singleton<GameManager>
{
    const int MAX_PLAYER = 4;

    private List<PlayerData> playerData;
    public List<PlayerData> GetAllPlayer() => playerData;
    public int NumberOfPlayers() => playerData.Count;
    public event UnityAction<PlayerData> OnPlayerAdded;
    public event UnityAction<PlayerData> OnPlayerRemoved;
    
    
    public void AddPlayer(InputDevice id)
    {
        PlayerData newPlayerData = new PlayerData(playerData.Count, id);
        playerData.Add(newPlayerData);
        OnPlayerAdded?.Invoke(newPlayerData);
    }
    public void RemovePlayer(InputDevice id)
    {
        PlayerData playerToRemove = playerData.Find(p => p.Device == id);
        if (playerToRemove != null)
        {
            playerData.Remove(playerToRemove);
            OnPlayerRemoved?.Invoke(playerToRemove);
            ReassignPlayerIndices();
        }
    }

    public void ChangeSkin(PlayerData pl, bool nextSkin = true)
    {
        if (!playerData.Contains(pl)) return;
        Debug.Log($"Next skin assigned for Player{pl.Index}");
    }

    private void ReassignPlayerIndices()
    {
        
    }

    protected override void Awake()
    {
        base.Awake();
        playerData = new List<PlayerData>();
    }
}

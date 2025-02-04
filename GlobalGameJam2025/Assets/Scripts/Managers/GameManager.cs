using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    //Script encargado de la conexion/desconexion de mandos
    public PlayerConnection PlayerConnection { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PlayerConnection = gameObject.AddComponent<PlayerConnection>();
        PlayerConnection.Init(this);
    }

    #region PLAYER MANAGEMENT

    public List<PlayerData> PlayersConnected { get; private set; } = new List<PlayerData>();
    public int PlayerCount => PlayersConnected.Count;

    public int GetPlayerIndex(PlayerData data)
    {
        if (data == null) return -1;
        return PlayersConnected.IndexOf(data);
    }

    // Para cambiar de Skins/Datos/etc.
    public void SetPlayerData(int playerIndex, PlayerData newPlayerData)
    {
        if (PlayersConnected[playerIndex] != null)
        {
            PlayersConnected[playerIndex] = newPlayerData;
        }
    }

    public void AddPlayer(PlayerData playerData)
    {
        if (playerData == null) return;

        PlayersConnected.Add(playerData);
        EventBus<PlayerAddedEvent>.Raise(new PlayerAddedEvent { data = playerData });
    }

    public void RemovePlayer(PlayerData playerData)
    {
        if (playerData == null) return;

        PlayersConnected.Remove(playerData);
        EventBus<PlayerRemovedEvent>.Raise(new PlayerRemovedEvent { data = playerData });
    }

    public void RemovePlayer(int index)
    {
        if (PlayersConnected[index] != null)
        {
            RemovePlayer(PlayersConnected[index]);
        }
    }

    public void ClearAllPlayers()
    {
        PlayersConnected.Clear();
        PlayerConnection.Init(this); // Reinitialize player connection
    }

    #endregion

    #region GAME DATA MANAGEMENT

    public GameData CurrentGame { get; private set; }

    public void CreateGameData(GameData newGameData)
    {
        if (newGameData == null) return;

        newGameData.SetPlayers(PlayersConnected);
        CurrentGame = newGameData;
    }

    public void CreateGameData(GameData newGameData, List<PlayerData> allPlayers)
    {
        if (newGameData == null) return;

        newGameData.SetPlayers(allPlayers);
        CurrentGame = newGameData;
    }

    public void AssignPoints(PlayerData player, int points = 0)
    {
        if (CurrentGame != null)
        {
            CurrentGame.SetPlayerPoints(player, points);
        }
    }

    #endregion
}

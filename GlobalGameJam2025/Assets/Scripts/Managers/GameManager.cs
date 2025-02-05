using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using System.Linq;
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

    //Data se encarga de los datos de los jugadores(Input que utilizan, color, etc.)
    public List<PlayerData> PlayersConnected { get; private set; } = new List<PlayerData>();
    public int PlayerCount => PlayersConnected.Count;

    public int GetPlayerIndex(PlayerData data) => data ? PlayersConnected.IndexOf(data) : -1;
    public PlayerData GetPlayer(int playerIndex) => PlayersConnected[playerIndex] ? PlayersConnected[playerIndex] : null;

    // Para cambiar de Skins/Datos/etc.
    public void SetPlayerData(int playerIndex, PlayerData newPlayerData)
    {
        if (PlayersConnected[playerIndex] != null)
        {
            PlayersConnected[playerIndex] = newPlayerData;
        }
    }
    public void SetNextPlayerSkin(PlayerData player)
    {
        if (player == null) return;
        PlayerSkin newSkin = PlayerSkin.GetNextAvailableSkin(player.GetSkin());
        player.SetSkin(newSkin);
    }

    //Se llaman desde PlayerConnection
    public void AddPlayer(PlayerData playerData)
    {
        PlayersConnected.Add(playerData);
        EventBus<PlayerConnectionEvent>.Raise(new PlayerConnectionEvent
        {
            conType = ConnectionType.Connected, data = playerData
        });
    }
    public void RemovePlayer(PlayerData playerData)
    {
        PlayersConnected.Remove(playerData);
        EventBus<PlayerConnectionEvent>.Raise(new PlayerConnectionEvent
        { 
            conType = ConnectionType.Disconnected, data = playerData
        });
    }

    public void ClearAllPlayers()
    {
        PlayersConnected.Clear();
        //PlayerConnection.Init(this); // Reinitialize player connection
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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerFX))]
[RequireComponent(typeof(PlayerInputHandler))]

public class PlayerCore : MonoBehaviour
{
    //References
    PlayerData playerData;
    PlayerFX playerFX;
    PlayerController playerController;
    PlayerInputHandler playerInput;

    public Action OnPlayerDash;

    public PlayerData PlayerData { get { return playerData; } }
    public PlayerFX PlayerFX { get { return playerFX; } }
    public PlayerController PlayerController { get { return playerController; } }
    public PlayerInputHandler PlayerInput { get { return playerInput; } }

    
    private static List<PlayerCore> allPlayers = new List<PlayerCore>();
    public static List<PlayerCore> AllPlayers
    {
        get
        {
            allPlayers.RemoveAll(player => player == null);
            return allPlayers;
        }
        private set { allPlayers = value; }
    }
    public void InitPlayer(PlayerData data, PlayerActionType mapType = PlayerActionType.Gameplay)
    {
        playerData = data;
        playerController = GetComponent<PlayerController>();
        playerFX = GetComponent<PlayerFX>();
        playerInput = GetComponent<PlayerInputHandler>();

        playerController.Init(playerData);
        playerFX.Init(playerData);
        PlayerInput.Init(data, mapType);

        //Auto add a la lista de Cores cuando todo esta ready
        allPlayers.Add(this);
    }

    #region Wrappers
    public void AddForce(Vector3 forceDir) => playerController.AddForce(forceDir);
    public void ToggleMovement(bool value)
    {
        playerController.ChangeState(value ?
            PlayerController.PlayerState.CanMove :
            PlayerController.PlayerState.Locked);
    }
    private void OnDestroy()
    {
        if (allPlayers.Contains(this)) allPlayers.Remove(this);
    }
    public void KillPlayer()
    {
        if(allPlayers.Contains(this)) allPlayers.Remove(this);
        playerController.KillPlayer();
        PlayerFX.KillPlayer();
        Destroy(gameObject, 1.0f);
    }
    #endregion
}
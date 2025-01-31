using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerFX))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerCore : MonoBehaviour
{
    //References
    PlayerData playerData;
    PlayerFX playerFX;
    PlayerController playerController;
    NewPlayerInput playerInput;


    public PlayerData PlayerData { get { return playerData; } }
    public PlayerFX PlayerFX { get { return playerFX; } }
    public PlayerController PlayerController { get { return playerController; } }
    public NewPlayerInput PlayerInput { get { return playerInput; } }

    public void InitPlayer(PlayerData data)
    {
        playerData = data;
        playerController = GetComponent<PlayerController>();
        playerFX = GetComponent<PlayerFX>();
        playerInput = GetComponent<NewPlayerInput>();

        playerController.Init(playerData);
        playerFX.Init(playerData);
    }

    #region Wrappers
    public void ToggleMovement(bool value)
    {
        playerController.ChangeState(value ?
            PlayerController.PlayerState.CanMove :
            PlayerController.PlayerState.Waiting);
    }
    public void KillPlayer()
    {
        playerController.KillPlayer();
        PlayerFX.KillPlayer();
        Destroy(gameObject, 1.0f);
    }
    #endregion
}
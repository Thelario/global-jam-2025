using System;
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


    public PlayerData PlayerData { get { return playerData; } }
    public PlayerFX PlayerFX { get { return playerFX; } }
    public PlayerController PlayerController { get { return playerController; } }
    public PlayerInputHandler PlayerInput { get { return playerInput; } }

    public void InitPlayer(PlayerData data)
    {
        playerData = data;
        playerController = GetComponent<PlayerController>();
        playerFX = GetComponent<PlayerFX>();
        playerInput = GetComponent<PlayerInputHandler>();

        playerController.Init(playerData);
        playerFX.Init(playerData);
        PlayerInput.Init(data, playerController);
    }

    #region Wrappers
    public void AddForce(Vector3 forceDir) => playerController.AddForce(forceDir);
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
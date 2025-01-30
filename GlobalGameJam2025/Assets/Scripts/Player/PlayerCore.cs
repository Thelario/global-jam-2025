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
    PlayerInput playerInput;


    public PlayerData PlayerData { get { return playerData; } }
    public PlayerInput PlayerInput { get { return playerInput; } }

    public void InitPlayer(PlayerData data)
    {
        playerData = data;
        playerController = GetComponent<PlayerController>();
        playerFX = GetComponent<PlayerFX>();
        playerInput = GetComponent<PlayerInput>();

        playerController.Init(playerData);
        playerFX.Init(playerData);
    }
}

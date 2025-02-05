using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerActionType
{
    Lobby,
    Gameplay
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerData playerData;
    private PlayerInput playerInput;
    private PlayerController playerController;
    private PlayerFX playerFX;

    public void Init(PlayerData data, PlayerActionType actionType = PlayerActionType.Gameplay)
    {
        playerData = data;
        playerController = GetComponent<PlayerController>();
        playerFX = GetComponent<PlayerFX>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.SwitchCurrentControlScheme(data.GetDeviceType());
        //playerInput.SwitchCurrentActionMap(actionType.ToString());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerController.MoveInput(context.ReadValue<Vector2>());
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            if(playerController) playerController.OnPlayerDash();
            if(playerFX) playerFX.OnPlayerDash();
            EventBus<PlayerDash>.Raise(new PlayerDash { data = playerData });
        }
    }
    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            if (playerController) playerController.OnPlayerSpecial();
            if (playerFX) playerFX.OnPlayerSpecial();
            EventBus<PlayerSpecial>.Raise(new PlayerSpecial { data = playerData });
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerActionType
{
    Menu,
    Gameplay
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;

    public void Init(PlayerData data, PlayerController pl, PlayerActionType actionType = PlayerActionType.Gameplay)
    {
        playerController = pl;
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(data.GetDeviceType());
        BindActions();
    }

    private void BindActions()
    {
        if (!playerInput) return;

        playerInput.onActionTriggered += HandleAction;
    }

    private void HandleAction(InputAction.CallbackContext context)
    {
        switch(context.action.name) 
        {
            case "Move":
                playerController.MoveInput(context.ReadValue<Vector2>());
                break;
            case "Dash":
                if (context.action.WasPressedThisFrame())
                {
                    playerController.Dash();
                }
                break;
        }
    }
}

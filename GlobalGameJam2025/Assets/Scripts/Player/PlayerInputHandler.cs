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
    private InputActionAsset inputActions;
    private PlayerController playerController;
    private PlayerInput playerInput;

    public void Init(PlayerController pl, PlayerActionType actionType)
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = pl;
        SetInputActions(actionType);
    }

    public void SetInputActions(PlayerActionType actionType)
    {
        Debug.Log("Setup Started: " + playerInput.currentActionMap.name);
        switch (actionType)
        {
            case PlayerActionType.Menu:
                playerInput.SwitchCurrentActionMap("Menu");
                break;
            case PlayerActionType.Gameplay:
                playerInput.SwitchCurrentActionMap("Gameplay");
                break;
        }
        Debug.Log("Setup FInished: " + playerInput.currentActionMap.name);
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += HandleInput;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= HandleInput;
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            playerController?.MoveInput(context.ReadValue<Vector2>());
            Debug.Log("MOVING");
        }
        else if (context.action.name == "Dash")
        {
            playerController?.Dash_Input(context);
        }
        else if (context.action.name == "Special")
        {
            Debug.Log("Game Special!");
        }
    }
}

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
    private Action onSpecial;
    private Action onDash;


    private PlayerData playerData;
    private PlayerInput playerInput;
    private PlayerController playerController;

    public void Init(PlayerData data, PlayerController pl, PlayerActionType actionType = PlayerActionType.Gameplay)
    {
        playerData = data;
        playerController = pl;

        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(data.GetDeviceType());
        
        BindActions(actionType);
    }
    public void InitCallbacks(Action notifyDash, Action notifySpecial)
    {
        onDash = notifyDash;
        onSpecial = notifySpecial;
    }
    private void OnDisable()
    {
        playerInput.onActionTriggered -= HandleAction;
    }

    private void BindActions(PlayerActionType actionType)
    {
        if (!playerInput) return;
        playerInput.SwitchCurrentActionMap(actionType.ToString());
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
                if (context.action.WasPerformedThisFrame())
                {
                    EventBus<PlayerDash>.Raise(new PlayerDash { data = playerData });
                    onDash?.Invoke();
                }
                break;
            case "Special":
                if (context.action.WasPerformedThisFrame())
                {
                    EventBus<PlayerSpecial>.Raise(new PlayerSpecial { data = playerData });
                    onSpecial?.Invoke();
                }
                break;
        }
    }
}

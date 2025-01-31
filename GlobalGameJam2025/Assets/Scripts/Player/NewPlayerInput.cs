using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum PlayerAction
{
    Dash,
    Special
}

[RequireComponent(typeof(PlayerInput))]
public class NewPlayerInput : MonoBehaviour
{
    // Events for different actions
    public event Action<Vector2> OnMoveInput;
    public event Action OnDashInput;
    public event Action OnSpecialInput;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Directly bind to actions
        playerInput.actions["Move"].performed += context => OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        playerInput.actions["Dash"].started += context => OnDashInput?.Invoke();
        playerInput.actions["Special"].started += context => OnSpecialInput?.Invoke();
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled
        playerInput.actions["Move"].performed -= context => OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        playerInput.actions["Dash"].started -= context => OnDashInput?.Invoke();
        playerInput.actions["Special"].started -= context => OnSpecialInput?.Invoke();
    }

    // Simplified method to subscribe to button actions with a callback
    public void SubscribeToInput(PlayerAction actionTarget, Action methodToTrigger)
    {
        InputAction inputAction = actionTarget switch
        {
            PlayerAction.Dash => playerInput.actions["Dash"],
            PlayerAction.Special => playerInput.actions["Special"],
            _ => null
        };

        if (inputAction != null)
            inputAction.started += context => methodToTrigger?.Invoke();
    }
    public void UnsubscribeToInput(PlayerAction actionTarget, Action methodToTrigger)
    {
        InputAction inputAction = actionTarget switch
        {
            PlayerAction.Dash => playerInput.actions["Dash"],
            PlayerAction.Special => playerInput.actions["Special"],
            _ => null
        };

        if (inputAction != null)
            inputAction.started -= context => methodToTrigger?.Invoke();
    }


    // Get input directly for use with CharacterController or other logic
    public Vector2 GetMoveInput() => playerInput.actions["Move"].ReadValue<Vector2>();
}

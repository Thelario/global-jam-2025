using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerInput playerInput;

    public void Init(PlayerController pl)
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = pl;
        BindActions();
    }

    private void BindActions()
    {
        if (!playerInput) return;
        // Bind Input Actions
        playerInput.actions["Move"].performed += OnMoveInput;
        playerInput.actions["Move"].canceled += OnMoveInput;
        playerInput.actions["Dash"].performed += OnDashInput;
    }

    private void OnDestroy()
    {
        if (!playerInput) return;
        // Unbind Input Actions
        playerInput.actions["Move"].performed -= OnMoveInput;
        playerInput.actions["Move"].canceled -= OnMoveInput;
        playerInput.actions["Dash"].performed -= OnDashInput;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        playerController.MoveInput(context.ReadValue<Vector2>());
    }

    private void OnDashInput(InputAction.CallbackContext context)
    {
        playerController.Dash_Input(context);
    }
    //private void Update()
    //{
    //    if (true)
    //    {
    //        playerController.MoveInput(new Vector2(Random.Range(-1,1),
    //            Random.Range(-1, 1)).normalized);
    //    }
    //}
}

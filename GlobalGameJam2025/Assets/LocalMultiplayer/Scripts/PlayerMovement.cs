using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    MultiplayerInstance localMultiplayerInstance;
    Vector2 moveInput;

    void Awake()
    {
        localMultiplayerInstance = GetComponent<MultiplayerInstance>();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInput.x = context.ReadValue<Vector2>().x;
        moveInput.y = context.ReadValue<Vector2>().y;

        Debug.Log(PlayerIndexString() + " : moveInput X = " + moveInput.x + " // Y = " + moveInput.y);
    }

    public virtual void JumpInput(InputAction.CallbackContext context)
    {
        Debug.Log(PlayerIndexString() + " : Jump");
    }

    string PlayerIndexString()
    {
        return "Player_" + 0;// localMultiplayerInstance.playerIndex;
    }
}

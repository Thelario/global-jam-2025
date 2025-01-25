using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerConnection : MonoBehaviour
{
    private List<InputDevice> connectedDevices = new List<InputDevice>();
    [SerializeField] private bool AlwaysAddKeyboard = true;
    private InputAction controllerAction; // The action to detect the button press

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        // Create an action map and add a listener to the action
        controllerAction = new InputAction("ControllerPress", binding: "<Gamepad>/buttonSouth");
        controllerAction.performed += _ => TryAddControllerPlayer();
        controllerAction.Enable();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;

        controllerAction.Disable();
    }

    private void Start()
    {
        if(GameManager.Instance.GetAllPlayer().Count > 0) Destroy(this.gameObject);
        if (AlwaysAddKeyboard && Keyboard.current != null)
        {
            AddPlayer(Keyboard.current);
        }

        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad gamepad && !connectedDevices.Contains(gamepad))
            {
                AddPlayer(gamepad);
            }
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                AddPlayer(device);
                break;

            case InputDeviceChange.Removed:
                RemovePlayer(device);
                break;
        }
    }

    // This method will try to add a controller when the button is pressed
    private void TryAddControllerPlayer()
    {
        // Check if the gamepad is available and if it hasn't been added already
        var gamepad = Gamepad.current;
        if (gamepad != null && !connectedDevices.Contains(gamepad))
        {
            AddPlayer(gamepad);
        }
    }

    public void AddPlayer(InputDevice device)
    {
        connectedDevices.Add(device);
        GameManager.Instance.AddPlayer(device);
        Debug.Log($"Player {device.displayName} added at index {connectedDevices.Count - 1}");
    }

    private void RemovePlayer(InputDevice device)
    {
        connectedDevices.Remove(device);
        GameManager.Instance.RemovePlayer(device);
        Debug.Log($"Player {device.displayName} removed.");
    }
}

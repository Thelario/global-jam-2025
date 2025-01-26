using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerConnection : MonoBehaviour
{
    private List<InputDevice> connectedDevices = new List<InputDevice>();
    [SerializeField] private bool AlwaysAddKeyboard = true;
    [SerializeField] private Transform rend;
    private InputAction controllerAction; // The action to detect the button press

    private void OnEnable()
    {
        if (GameManager.Instance.controllersInitialized)
        {
            Destroy(this);  
            return;
        }

        GameManager.Instance.controllersInitialized = true;
        DontDestroyOnLoad(gameObject);  

        InputSystem.onDeviceChange += OnDeviceChange;

        controllerAction = new InputAction("ControllerPress", binding: "<Gamepad>/buttonSouth");
        controllerAction.performed += _ => TryAddControllerPlayer();
        controllerAction.Enable();
    }

    private void OnDisable()
    {
        if (controllerAction != null)
        {
            controllerAction.Disable();
        }
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void Start()
    {
        // if (AlwaysAddKeyboard && Keyboard.current != null)
        // {
        //     AddPlayer(Keyboard.current);
        // }

        // foreach (var device in InputSystem.devices)
        // {
        //     if (device is Gamepad gamepad && !connectedDevices.Contains(gamepad))
        //     {
        //         AddPlayer(gamepad);
        //     }
        // }
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
        if (device is Keyboard) return;
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

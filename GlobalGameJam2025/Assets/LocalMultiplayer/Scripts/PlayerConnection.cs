using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerConnection : MonoBehaviour
{
    private List<InputDevice> connectedDevices = new List<InputDevice>();
    [SerializeField] private bool AlwaysAddKeyboard = true;
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void Awake()
    {
        if (AlwaysAddKeyboard && Keyboard.current != null)
        {
            AddPlayer(Keyboard.current);
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

    private void AddPlayer(InputDevice device)
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
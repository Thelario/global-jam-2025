using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class PlayerConnection : MonoBehaviour
{
    private GameManager gameManager;

    private void OnEnable() => InputSystem.onDeviceChange += OnDeviceChange;
    private void OnDisable() => InputSystem.onDeviceChange -= OnDeviceChange;

    public void Init(GameManager manager)
    {
        gameManager = manager;

        if (GameSettings.INITIALIZE_CONTROLLERS)
        {
            foreach (var connPl in GetAllConnectedDevices())
            {
                AddPlayer(connPl);
            }
        }

        // Optionally simulate players if needed
        // if (GameSettings.SIMULATE_PLAYERS != 0) AddFakePlayers();
    }

    private void Update()
    {
        HandleDeviceInput();
    }

    private void HandleDeviceInput()
    {
        // Connect controller on START
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            AddPlayer(Gamepad.current);
        }

        // Connect keyboard on ENTER
        if (Input.GetKey(KeyCode.Return))
        {
            AddPlayer(Keyboard.current);
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added)
        {
            if (device is Gamepad || device is Keyboard)
                AddPlayer(device);
        }
        else if (change == InputDeviceChange.Removed)
        {
            if (device is Gamepad || device is Keyboard)
                RemovePlayer(device);
        }
    }

    // Check if player already exists by device ID
    public bool PlayerExists(InputDevice device)
    {
        return gameManager.PlayersConnected.Exists(p => p.GetID() == device.deviceId);
    }

    private void AddPlayer(InputDevice device)
    {
        if (device == null || PlayerExists(device)) return;

        PlayerData newPlayerData = new PlayerData(device, GetPlayerSkin());
        gameManager.AddPlayer(newPlayerData);
    }

    private void RemovePlayer(InputDevice device)
    {
        if (device == null) return;

        PlayerData playerToRemove = gameManager.PlayersConnected
            .FirstOrDefault(p => p.GetID() == device.deviceId);

        if (playerToRemove != null)
        {
            gameManager.RemovePlayer(playerToRemove);
        }
    }

    public List<InputDevice> GetAllConnectedDevices()
    {
        return InputSystem.devices
            .Where(device => device is Keyboard || device is Gamepad)
            .ToList();
    }

    private PlayerSkin GetPlayerSkin()
    {
        return GameSettings.ASSIGN_SKINS_IN_ORDER ?
            PlayerSkin.GetFirstAvailableSkin(gameManager.PlayersConnected) :
            PlayerSkin.GetRandomUnassignedSkin(gameManager.PlayersConnected);
    }
}

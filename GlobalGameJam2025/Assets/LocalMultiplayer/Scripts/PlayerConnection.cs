using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class PlayerConnection : MonoBehaviour
{
    private bool AlwaysAddKeyboard = true;
    GameManager gameManager;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (AlwaysAddKeyboard && Keyboard.current != null) AddPlayer(Keyboard.current);
    }
    private void Update()
    {
        //Conectar mando on START
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            AddPlayer(Gamepad.current);
        }
        if (Input.GetKey(KeyCode.Return)) //Conectar Teclado on ENTER
        {
            AddPlayer(Keyboard.current);
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                if (device is Gamepad || device is Keyboard)
                {
                    AddPlayer(device);
                }
                break;

            case InputDeviceChange.Removed:
                if (device is Gamepad || device is Keyboard)
                {
                    RemovePlayer(device);
                }
                break;
        }
    }

    // Checkea si un jugador ya esta conectado por ID de mando
    public bool PlayerExists(InputDevice device)
    {
        return gameManager.GetPlayerList().Exists(p => p.GetID() == device.deviceId);
    }
    private void AddPlayer(InputDevice device)
    {
        if (device == null) return;

        if (PlayerExists(device)) return;

        PlayerSkin availableSkin = PlayerSkin.GetFirstAvailableSkin(gameManager.GetPlayerList());
        PlayerData newPlayerData = new PlayerData(device, availableSkin);

        gameManager.AddPlayer(newPlayerData);
    }

    private void RemovePlayer(InputDevice device)
    {
        if (device == null) return;

        PlayerData playerToRemove = gameManager.GetPlayerList()
            .FirstOrDefault(p => p.GetID() == device.deviceId);

        if (playerToRemove != null) gameManager.RemovePlayer(playerToRemove);
    }
}

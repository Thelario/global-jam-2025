using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Clase que se encarga de detectar conexiones de mandos/teclados
/// Se anade por defecto al GameManager en GameManager Init
/// </summary>

public class PlayerConnection : MonoBehaviour
{
    GameManager gameManager;

    private void OnEnable() => InputSystem.onDeviceChange += OnDeviceChange;
    private void OnDisable() => InputSystem.onDeviceChange -= OnDeviceChange;

    public void Init()
    {
        gameManager = GameManager.Instance;
        if (GameSettings.ALWAYS_CREATE_KEYBOARD && Keyboard.current != null) AddPlayer(Keyboard.current);
        foreach(var connPl in GetAllConnectedDevices())
        {
            Debug.Log("CONECTED");
            AddPlayer(connPl);
        }
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
        if(change == InputDeviceChange.Added)
        {
            if (device is Gamepad || device is Keyboard) AddPlayer(device);
        }
        else if(change == InputDeviceChange.Removed)
        {
            if (device is Gamepad || device is Keyboard) RemovePlayer(device);
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

        PlayerData newPlayerData = new PlayerData(device, GetPlayerSkin());
        gameManager.AddPlayer(newPlayerData);
    }

    private void RemovePlayer(InputDevice device)
    {
        if (device == null) return;

        PlayerData playerToRemove = gameManager.GetPlayerList()
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
            PlayerSkin.GetFirstAvailableSkin(gameManager.GetPlayerList()) :
            PlayerSkin.GetRandomUnassignedSkin(gameManager.GetPlayerList());
    }
}

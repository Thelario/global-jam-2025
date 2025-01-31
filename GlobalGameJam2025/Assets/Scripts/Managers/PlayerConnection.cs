using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Clase que se encarga de detectar conexiones de mandos/teclados
/// Se anade por defecto por el GameManager en su Init
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
    //Principalmente se utiliza para Testear minijuegos desde TestMinigame.cs
    //De otra forma, se espera al input de Start/Enter o lo que sea. No llama a GameManager
    public List<PlayerData> GetAllConnectedDevices()
    {
        List<InputDevice> allConnections = new List<InputDevice>(InputSystem.devices);
        List<PlayerData> allData = new List<PlayerData>();
        foreach(var dev in allConnections)
        {
            if (PlayerExists(dev)) continue;
            PlayerData newPlayerData = new PlayerData(dev, GetPlayerSkin());
            allData.Add(newPlayerData);
        }
        return allData;
    }
    private PlayerSkin GetPlayerSkin()
    {
        return GameSettings.ASSIGN_SKINS_IN_ORDER ?
            PlayerSkin.GetFirstAvailableSkin(gameManager.GetPlayerList()) :
            PlayerSkin.GetRandomUnassignedSkin(gameManager.GetPlayerList());
    }
}

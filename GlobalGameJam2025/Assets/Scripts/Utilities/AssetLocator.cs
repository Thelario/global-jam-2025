using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;


/// <summary>
/// Clase statica generica para referencias (en general constantes)
/// Para referencias directas de un asset, hacerlo del modo:
/// AssetLocator.Data.(ReferenciaImagen.png)
/// </summary>
public static class AssetLocator
{
    public static AssetData Data { get; private set; }
    static AssetLocator() => LoadAssetData();

    private static void LoadAssetData()
    {
        Data = Resources.Load<AssetData>("AssetData");
        if (Data == null) Debug.LogError("AssetData not found in Resources!");
    }


    #region Player Skins
    public static PlayerSkin[] AllSkins()
    {
        return m_allPlayers;
    }
    private static PlayerSkin[] m_allPlayers => Resources.LoadAll<PlayerSkin>("Skins");
    #endregion

    #region UI
    public static Sprite GetControllerIcon(InputDevice device)
    {
        int index = device is Keyboard ? 0 :
                    device is Gamepad gamepad ?
                    gamepad is XInputController ? 1 :
                    gamepad is DualShockGamepad ? 2 :
                    gamepad is SwitchProControllerHID ? 3 : 0
                    : 0;
        return Data.ControllerIconSprites[index];
    }
    #endregion

    

}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AssetLocator", menuName = "Managers/AssetLocator", order = 1)]
public static class AssetLocator
{
    private static AssetData assetData;
    static AssetLocator() => LoadAssetData();

    private static void LoadAssetData()
    {
        assetData = Resources.Load<AssetData>("AssetData");
        if (assetData == null) Debug.LogError("AssetData not found in Resources!");
    }
    public static List<Color> PaintColors => new List<Color>()
    {
        new Color32(255,191,97,255),
        new Color32(255,81,113,255),
        new Color32(185,213,47,255),
        new Color32(85,166,255,255)
    };
    //SKINS
    public static PlayerSkin DefaultSkin => assetData.DefaultSkin;
    public static PlayerSkin[] AllSkins()
    {
        return Resources.LoadAll<PlayerSkin>("Skins");
    }
    public static Sprite GetControllerIcon(InputDevice device)
    {
        Debug.Log("Short:" + device.shortDisplayName);
        Debug.Log("Display:" + device.displayName);
        int index = device is Keyboard ? 0 :
                    device is Gamepad gamepad ?
                    gamepad is XInputController ? 1 :
                    gamepad is DualShockGamepad ? 2 :
                    gamepad is SwitchProControllerHID ? 3 : 0
                    : 0;
        return assetData.ControllerIconSprites[index];
    }


    public static List<MinigameBase> ALLGAMES => assetData.ALLGAMES;
    public static Fader Fader => assetData.Fader;
    public static Texture2D MinigameEditorIcon => assetData.MinigameEditorIcon;
    public static GameObject MainCanvasPrefab => assetData.MainCanvasPrefab;
    public static GameObject PlayerPrefab() => assetData.PlayerPrefab;
}
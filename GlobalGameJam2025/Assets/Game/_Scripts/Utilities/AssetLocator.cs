using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

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
        new Color32(218,109,210,255),
        new Color32(240,59,110,255),
        new Color32(180,210,61,255),
        new Color32(70,120,240,255)
    };
    public static List<MinigameBase> ALLGAMES => assetData.ALLGAMES;
    public static Fader Fader => assetData.Fader;
    public static Texture2D MinigameEditorIcon => assetData.MinigameEditorIcon;
    public static GameObject MainCanvasPrefab => assetData.MainCanvasPrefab;
    public static GameObject PlayerPrefab() => assetData.PlayerPrefab;
}
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

    public static GameObject PlayerPrefab => assetData.PlayerPrefab;
    public static Texture2D MinigameEditorIcon => assetData.MinigameEditorIcon;
}
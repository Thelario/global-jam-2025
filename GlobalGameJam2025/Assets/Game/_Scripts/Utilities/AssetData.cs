using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetData", menuName = "Managers/AssetData", order = 0)]
public class AssetData : ScriptableObject
{
    public GameObject PlayerPrefab;
    public Texture2D MinigameEditorIcon;
}

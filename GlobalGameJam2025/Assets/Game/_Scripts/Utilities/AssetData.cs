using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetData", menuName = "Managers/AssetData", order = 0)]
public class AssetData : ScriptableObject
{
    public List<MinigameBase> ALLGAMES;
    [Header("GAMEOBJECTS")] 
    public GameObject PlayerPrefab;
    
    [Space(20)]
    [Header("UI")]
    public GameObject MainCanvasPrefab;
    public Fader Fader;

    [Space(20)]
    [Header("Editor")]
    public Texture2D MinigameEditorIcon;
}

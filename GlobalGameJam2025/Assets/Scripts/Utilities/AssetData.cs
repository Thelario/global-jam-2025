using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AssetData", menuName = "Managers/AssetData", order = 0)]
public class AssetData : ScriptableObject
{
    public List<MinigameData> ALLGAMES;
    [Header("PLAYER")] 
    public PlayerCore PlayerPrefab;
    public PlayerSkin DefaultSkin;
    
    [Space(20)]
    [Header("UI")]
    public GameObject MainCanvasPrefab;
    public Fader Fader;
    public Sprite[] ControllerIconSprites;

    [Space(20)]
    [Header("Editor")]
    public Texture2D MinigameEditorIcon;
}

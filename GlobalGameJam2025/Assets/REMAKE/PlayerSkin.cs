using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(fileName = "NewSkin", menuName = "Player/Skin")]
public class PlayerSkin : ScriptableObject
{
    public string skinName;
    public Color mainColor, secondaryColor;
    public Texture skinTexture;

    /// <summary>
    /// Finds the first available skin that is not assigned to any player.
    /// </summary>
    public static PlayerSkin GetFirstAvailableSkin(List<PlayerData> players)
    {
        PlayerSkin[] allSkins = Resources.LoadAll<PlayerSkin>("Skins");
        List<PlayerSkin> assignedSkins = players.Select(p => p.GetSkin()).ToList();

        return allSkins.FirstOrDefault(skin => !assignedSkins.Contains(skin));
    }
}

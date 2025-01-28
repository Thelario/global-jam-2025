using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkin", menuName = "Player/Skin")]
public class PlayerSkin : ScriptableObject
{
    public int Index = 0;
    public Color mainColor = new Color(1, 1, 1, 1);
    public Color secondaryColor = new Color(1,1,1,1);
    public Texture skinTexture;

    /// <summary>
    /// Finds the first available skin that is not assigned to any player.
    /// </summary>
    public static PlayerSkin GetFirstAvailableSkin(List<PlayerData> players)
    {
        PlayerSkin[] allSkins = AssetLocator.AllSkins();
        List<PlayerSkin> assignedSkins = players.Select(p => p.GetSkin()).ToList();
        allSkins = allSkins.OrderBy(skin => skin.Index).ToArray();
        return allSkins.FirstOrDefault(skin => !assignedSkins.Any(assigned => assigned.Index == skin.Index));
    }
}

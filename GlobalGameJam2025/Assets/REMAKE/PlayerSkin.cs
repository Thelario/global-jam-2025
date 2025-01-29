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
    /// Encuentra la primera skin que no esta siendo utilizada por otro jugador,
    /// basado en el PlayerSkin.Index (para siempre tener un orden por defecto (Amarillo->Rojo->Azul...))
    /// </summary>
    public static PlayerSkin GetFirstAvailableSkin(List<PlayerData> players)
    {
        PlayerSkin[] allSkins = AssetLocator.AllSkins();
        List<PlayerSkin> assignedSkins = players.Select(p => p.GetSkin()).ToList();
        allSkins = allSkins.OrderBy(skin => skin.Index).ToArray();
        foreach (var skin in allSkins)
        {
            if (!assignedSkins.Any(assigned => assigned.Index == skin.Index)) return skin;
        }
        return null;
    }
    /// <summary>
    /// Igual que arriba, pero ignorando index. No creo que se llegue a utilizar
    /// </summary>
    public static PlayerSkin GetRandomUnassignedSkin(List<PlayerData> players)
    {
        PlayerSkin[] allSkins = AssetLocator.AllSkins();
        List<PlayerSkin> assignedSkins = players.Select(p => p.GetSkin()).ToList();
        var unassignedSkins = allSkins.Where(skin => !assignedSkins.Any(assigned => assigned.Index == skin.Index)).ToList();
        if (unassignedSkins.Count == 0) return null;
        return unassignedSkins[UnityEngine.Random.Range(0, unassignedSkins.Count)];
    }

}

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
    public static PlayerSkin GetFirstAvailableSkin()
    {
        PlayerSkin[] allSkins = AssetLocator.AllSkins();
        List<PlayerSkin> assignedSkins = GameManager.Instance.PlayersConnected.Select(p => p.GetSkin()).ToList();
        allSkins = allSkins.OrderBy(skin => skin.Index).ToArray();
        foreach (var skin in allSkins)
        {
            if (!assignedSkins.Any(assigned => assigned.Index == skin.Index)) return skin;
        }
        return null;
    }/// <summary>
     /// Igual que arriba, pero coge la siguiente skin con index superior a la actual.
     /// Para ir recorriendo todas las skins de una en una al asignar skin
     /// </summary>
    public static PlayerSkin GetNextAvailableSkin(PlayerSkin current)
    {
        PlayerSkin[] allSkins = AssetLocator.AllSkins().OrderBy(skin => skin.Index).ToArray();
        List<PlayerSkin> assignedSkins = GameManager.Instance.PlayersConnected.Select(p => p.GetSkin()).ToList();

        foreach (var skin in allSkins)
        {
            if (skin.Index > current.Index && !assignedSkins.Any(assigned => assigned.Index == skin.Index))
            {
                return skin;
            }
        }
        return GetFirstAvailableSkin();
    }
    /// <summary>
    /// Igual que arriba, pero ignorando index. No creo que se llegue a utilizar
    /// </summary>
    public static PlayerSkin GetRandomUnassignedSkin()
    {
        PlayerSkin[] allSkins = AssetLocator.AllSkins();
        List<PlayerSkin> assignedSkins = GameManager.Instance.PlayersConnected.Select(p => p.GetSkin()).ToList();
        var unassignedSkins = allSkins.Where(skin => !assignedSkins.Any(assigned => assigned.Index == skin.Index)).ToList();
        if (unassignedSkins.Count == 0) return null;
        return unassignedSkins[UnityEngine.Random.Range(0, unassignedSkins.Count)];
    }

}

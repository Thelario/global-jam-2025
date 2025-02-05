using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class PlayerData
{
    private InputDevice m_device;
    private PlayerSkin m_skin;
    private int m_points;

    public PlayerData(InputDevice newDevice, PlayerSkin newSkin)
    {
        m_device = newDevice;
        m_skin = newSkin;
        m_points = 0;
    }

    public InputDevice GetDeviceType() => m_device;
    public int GetID() => m_device.deviceId;
    public PlayerSkin GetSkin() => m_skin;
    public void SetSkin(PlayerSkin newSkin) => m_skin = newSkin;
    public void AddPoints(int value) => m_points += value;
}

public static class PlayerDataExtensions
{
    // Extension method para comparar PlayeDatas
    public static bool IsEqualTo(this PlayerData playerData, PlayerData other)
    {
        return playerData.GetID() == other.GetID();
    }
    public static int GetHashCode(this PlayerData playerData)
    {
        int hash = 17;//Numero Primo para Hash
        hash = hash * 23 + playerData.GetID().GetHashCode();
        return hash;
    }
}
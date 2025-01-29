using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerFX))]

public class PlayerCore : MonoBehaviour
{
    //References
    PlayerData playerData;
    PlayerFX playerFX;
    PlayerController playerController;

    private void InitPlayer(PlayerData data)
    {
        playerData = data;
        playerController = GetComponent<PlayerController>();
        playerFX = GetComponent<PlayerFX>();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TestMinigame : MonoBehaviour
{
    [SerializeField] private MinigameData minigameData;
    private int pointsToWin = 10;

    private void Awake()
    {
        GameManager gameManager = GameManager.Instance;
        PlayerConnection playerConn = gameManager.PlayerConnection;

        GameData newGameData = new GameData(new List<MinigameData>() { minigameData },
            pointsToWin);
        gameManager.CreateGameData(newGameData, playerConn.GetAllConnectedDevices());
    }
}

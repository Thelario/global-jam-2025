using System.Collections.Generic;
using UnityEngine;

public class TestMinigame : MonoBehaviour
{
    [SerializeField] private MinigameData minigameData;
    private int pointsToWin = 10;

    private void OnEnable()
    {
        GameManager gameManager = GameManager.Instance;
        PlayerConnection playerConn = gameManager.PlayerConnection;

        GameData newGameData = new GameData(new List<MinigameData>() { minigameData },
            pointsToWin);
        GameManager.Instance.CreateGameData(newGameData, playerConn.GetAllConnectedDevices());
    }
}

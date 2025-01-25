using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnPointsManager : Singleton<EarnPointsManager>
{
    GameplayMultiplayerManager gameplayMultiplayerManager;

    [SerializeReference] PlayerPointTower[] playerPointTower;

    [SerializeField] Transform finishLine;

    int maxScore = 10;

    List<PlayerData> allPlayersConnected;

    List<PlayerData> playerResults;

    List<MultiplayerInstance> allPlayerInstances;

    protected override void Awake()
    {
        base.Awake();

        finishLine.position = new Vector3(finishLine.position.x, maxScore * PlayerPointTower.distanceBetweenChips, finishLine.position.z);

        allPlayersConnected = GameManager.Instance.GetAllPlayer();

        //playerResults = MinigameManager.Instance.GetResults;

        StartCoroutine(EarnPointsSequence());
    }

    IEnumerator EarnPointsSequence()
    {
        //MinigameManager.Instance.SpawnPlayers();

        playerResults = MinigameManager.Instance.GetLastGameScore();

        //if (playerResults.Count == 0)
        //{
        //    int efdrgt = 0;
        //}

        for (int i = 0; i < playerResults.Count; i++)
        {
            PlayerData currentPlayer = playerResults[i];

            PlayerPointTower tower = playerPointTower[currentPlayer.Index];
            if (tower != null)
            {
                for (int j = 0; j < currentPlayer.TotalPoints; j++)
                    tower.AddChip(false);
            }
        }

        for (int i = 0; i < playerResults.Count; i++)
        {
            PlayerData currentPlayer = playerResults[i];

            int pointsAdded = i;
            playerResults[i].SetTotalPoints(currentPlayer.TotalPoints + pointsAdded);

            PlayerPointTower tower = playerPointTower[currentPlayer.Index];
            if (tower != null)
            {
                yield return tower.AddChipsDelay(pointsAdded);

                yield return new WaitForSeconds(.5f);

                MinigameManager.Instance.SpawnPlayerByIndex(currentPlayer.Index, tower.GetPlayerSpawnPosition());
            }

            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(1);

        //TODO: MIRAR

        //GameplayMultiplayerManager.Instance.SpawnPlayers();
        //GameManager.Instance.ChangeScene("Gameplay");

        //yield return new WaitForSeconds(1);
        //if (MinigameManager.Instance.RoundsLeft() != 0)

        PlayerData winningPlayer;
        int topPoints = 0;

        for (int i = 0; i < playerResults.Count; i++)
        {
            if (playerResults[i].TotalPoints > topPoints)
            {
                winningPlayer = playerResults[i];
                topPoints = winningPlayer.TotalPoints;
                playerResults[i] = winningPlayer;
            }
        }

        if (topPoints >= maxScore)
            SceneNav.GoTo(SceneType.MainMenuScene);
        else
            SceneNav.GoTo(SceneType.Gameplay);
    }
}

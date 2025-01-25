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
        yield return new WaitForSeconds(1);

        //allPlayerInstances = GameManager.Instance.GetPlayerInstances();

        for (int i = 0; i < playerResults.Count; i++)
        {
            PlayerData currentPlayer = playerResults[i];

            PlayerPointTower tower = playerPointTower[currentPlayer.Index];
            if (tower != null)
            {
                yield return tower.AddChipsDelay(i);
                yield return new WaitForSeconds(1);
            }
        }

        //TODO: MIRAR

        //GameplayMultiplayerManager.Instance.SpawnPlayers();
        //GameManager.Instance.ChangeScene("Gameplay");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnPointsManager : Singleton<EarnPointsManager>
{
    GameplayMultiplayerManager gameplayMultiplayerManager;

    [SerializeReference] PlayerPointTower[] playerPointTower;

    //List<PlayerData> players;

    protected override void Awake()
    {
        base.Awake();

        // Acceder a los players
        //player = ...

        StartCoroutine(EarnPointsSequence());
    }

    IEnumerator EarnPointsSequence()
    {
        yield return new WaitForSeconds(1);

        // Recorrer los jugadores
        for (int i = 1; i >= 0; i--)
        {
            PlayerPointTower tower = playerPointTower[i];

            if (tower != null)
            {
                yield return tower.AddChipsDelay(1);
                yield return new WaitForSeconds(1);
            }
        }
        //TODO: MIRAR

        //GameplayMultiplayerManager.Instance.SpawnPlayers();

        GameManager.Instance.ChangeScene("Gameplay");
    }
}

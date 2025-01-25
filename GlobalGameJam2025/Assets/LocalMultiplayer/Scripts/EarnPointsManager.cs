using System.Collections;
using UnityEngine;

public class EarnPointsManager : Singleton<EarnPointsManager>
{
    GameplayMultiplayerManager gameplayMultiplayerManager;

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(EarnPointsSequence());
    }

    IEnumerator EarnPointsSequence()
    {
        yield return new WaitForSeconds(1);

        GameplayMultiplayerManager.Instance.SpawnPlayers();
    }
}

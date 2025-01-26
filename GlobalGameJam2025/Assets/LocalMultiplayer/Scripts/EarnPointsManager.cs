using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class EarnPointsManager : Singleton<EarnPointsManager>
{
    GameplayMultiplayerManager gameplayMultiplayerManager;

    [SerializeReference] PlayerPointTower[] playerPointTower;

    [SerializeField] Transform finishLine;

    int maxScore = 10;

    List<PlayerData> allPlayersConnected;

    List<PlayerData> playerResults;

    List<MultiplayerInstance> allPlayerInstances;


    [Serializable]
    struct TextStruct
    {
        public TMP_Text bigText;
        public TMP_Text smallText;
    }

    [SerializeField] TextStruct[] positionsText;

    [SerializeField] Color[] colorText = new Color[4];

    protected override void Awake()
    {
        base.Awake();

        finishLine.position = new Vector3(finishLine.position.x, maxScore * PlayerPointTower.distanceBetweenChips, finishLine.position.z);

        allPlayersConnected = GameManager.Instance.GetAllPlayer();

        //playerResults = MinigameManager.Instance.GetResults;

        StartCoroutine(EarnPointsSequence());

        //StartCoroutine(slkdjlskdjf());
    }

    IEnumerator slkdjlskdjf()
    {
        yield return new WaitForSeconds(2);

        positionIntToText(positionsText[0], 0);
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
                positionIntToText(positionsText[currentPlayer.Index], playerResults.Count - 1 - i);

                yield return tower.AddChipsDelay(pointsAdded);

                yield return new WaitForSeconds(.1f);

                MinigameManager.Instance.SpawnPlayerByIndex(currentPlayer.Index, tower.GetPlayerSpawnPosition());
            }

            if (i != playerResults.Count - 1)
                yield return new WaitForSeconds(1.5f);
        }

        //yield return new WaitForSeconds(1);

        //TODO: MIRAR

        //GameplayMultiplayerManager.Instance.SpawnPlayers();
        //GameManager.Instance.ChangeScene("Gameplay");

        //yield return new WaitForSeconds(1);
        //if (MinigameManager.Instance.RoundsLeft() != 0)

        PlayerData winningPlayer = null;
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

        ShowCrown();

        yield return new WaitForSeconds(2);

        if (topPoints >= maxScore)
            SceneNav.GoTo(SceneType.MainMenuScene);
        else
            SceneNav.GoTo(SceneType.Gameplay);
    }

    void ShowCrown()
    {
        PlayerData winningPlayer = null;
        int topPoints = 0;

        for (int i = 0; i < playerResults.Count; i++)
        {
            if (playerResults[i].TotalPoints > topPoints)
            {
                winningPlayer = playerResults[i];
                topPoints = winningPlayer.TotalPoints;
            }
        }

        MultiplayerInstance winningPlayerInstance = null;
        MultiplayerInstance[] multiplayerInstances = FindObjectsByType<MultiplayerInstance>(FindObjectsSortMode.None);
        for (int i = 0; i < multiplayerInstances.Length; i++)
        {
            if (multiplayerInstances[i].playerIndex == winningPlayer.Index)
            {
                winningPlayerInstance = multiplayerInstances[i];
                Crown.instance.playerFollow = winningPlayerInstance.transform;
            }
        }
    }

    void positionIntToText(TextStruct textStruct, int index)
    {
        switch (index)
        {
            case 0:
                textStruct.bigText.text = "1";
                textStruct.smallText.text = "st";
                textStruct.bigText.color = colorText[0];
                textStruct.smallText.color = colorText[0];
                break;
            case 1:
                textStruct.bigText.text = "2";
                textStruct.smallText.text = "nd";
                textStruct.bigText.color = colorText[1];
                textStruct.smallText.color = colorText[1];
                break;
            case 2:
                textStruct.bigText.text = "3";
                textStruct.smallText.text = "rd";
                textStruct.bigText.color = colorText[2];
                textStruct.smallText.color = colorText[2];
                break;
            case 3:
                textStruct.bigText.text = "4";
                textStruct.smallText.text = "th";
                textStruct.bigText.color = colorText[3];
                textStruct.smallText.color = colorText[3];
                break;
            default:
                break;
        }

        textStruct.bigText.DOFade(1, 1);
        textStruct.smallText.DOFade(1, 1);

        textStruct.bigText.transform.parent.transform.localScale = Vector3.zero;
        textStruct.bigText.transform.parent.DOScale(Vector3.one, .2f);
    }
}

using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ConfigGame : MonoBehaviour
{
    //Default Values megaguays
    private int indexGame = 0;
    private int indexRounds = 3;
    private int indexTimer = 1;

    private List<string> gameNames;
    private readonly int[] roundList = new int[] { 1,2,3,4,5,6};
    private readonly int[] timerList = new int[] { 30,40,60,999};

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI gameText;
    [SerializeField] private TextMeshProUGUI roundText, timerText;
    [Header("Players")]
    [SerializeField] private TextMeshProUGUI numberPlayers;
    [SerializeField] private List<CanvasGroup> playerList;
    
    
    private void Start()
    {
        if (!MinigameManager.Instance) return;
        gameNames = new List<string>(MinigameManager.GamesLoaded().Values);
        gameNames.Insert(0, "Random");

        //Players Connected
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].transform.DOScale(Vector3.one * 1.025f, 0.3f).SetDelay(i * UnityEngine.Random.Range(0.2f, 0.7f))
                .SetLoops(-1, LoopType.Yoyo);
        }
        if (!GameManager.instance) return;
        int connectedPlys = GameManager.instance.currentCharactersData.Length;
        numberPlayers.text = $"Players {connectedPlys}/4";
        for (int i = 4- connectedPlys; i >0;i--)
        {
            playerList[i].alpha = 0.2f;
        }
        
    }

    public void SelectGame(bool right)
    {
        indexGame += right ? 1 : -1;
        if (indexGame >= gameNames.Count) indexGame = 0;
        else if (indexGame < 0) indexGame = gameNames.Count - 1;
        gameText.text = gameNames[indexGame];
    }

    public void SelectRounds(bool right)
    {
        indexRounds += right ? 1 : -1;
        if (indexRounds >= roundList.Length) indexRounds = 0;
        else if (indexRounds < 0) indexRounds = roundList.Length - 1;
        roundText.text = roundList[indexRounds].ToString();
    }

    public void SelectTimer(bool right)
    {
        indexTimer += right ? 1 : -1;
        if (indexTimer >= timerList.Length) indexTimer = 0;
        else if (indexTimer < 0) indexTimer = timerList.Length - 1;
        timerText.text = $"{timerList[indexTimer].ToString()} <size=70%> sexs.";
    }
    public void ChangeScene()
    {
        MinigameManager.Instance.InitMinigameInfo(roundList[indexRounds], timerList[indexTimer], indexGame);
        SceneManager.LoadScene("Gameplay");
    }
}

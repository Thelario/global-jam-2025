using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigGame : MonoBehaviour
{
    //Default Values megaguays
    private int indexGame = 0;
    private int indexRounds = 3;
    private int indexTimer = 1;

    private List<string> gameNames;
    private readonly int[] roundList = new int[] { 1,2,3,4,5,6};
    private readonly int[] timerList = new int[] { 5,20,30,40,999};

    [Header("UI Texts")]
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI gameText;
    [SerializeField] private TextMeshProUGUI roundText, timerText;
    
    private void Start()
    {
        if(backButton) backButton.onClick.AddListener(()=> SceneNav.GoTo(SceneType.PlayerSelect));
        if (!MinigameManager.Instance) return;
        gameNames = new List<string>();
        foreach(var a in AssetLocator.ALLGAMES)
        {
            gameNames.Add(a.Name());
        }
        gameNames.Insert(0, "Random");
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
        timerText.text = $"{timerList[indexTimer].ToString()} <size=70%> sec.";
    }
    public void ChangeScene()
    {
        SoundManager.Instance.PlaySound(Sound.GameMusic);
        MinigameManager.Instance.InitMinigameInfo(roundList[indexRounds], timerList[indexTimer], indexGame);
        SceneNav.GoTo(SceneType.Gameplay);
    }
}

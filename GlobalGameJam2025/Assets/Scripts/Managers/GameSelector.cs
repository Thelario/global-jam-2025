using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameData;
public class GameSelector : MonoBehaviour
{
    //MAX CAPS/ VARIO
    //private int[] pointsToWinList = new int[] { 3,5,7,10,15};


    //Temporal Data antes de creacion de GameData
    //private GameData.GameMode gameMode = ;
    private int gameModeIndex = -1;
    private int pointsToWin = 10;
    private bool lastPlaceElim = false;
    private int lastPlacePenalty = 0;

    [SerializeField] private TextMeshProUGUI gamemodeText, pointsText, eliminationText, penaltyText;

    private bool startingGame = false;

    public void StartGame()//Called on Fire it up
    {
        if (startingGame) return;
        startingGame = true;
        AssignGameInfo();
        SceneNav.GoTo(SceneType.Gameplay);
    }
    private void AssignGameInfo() //TEMPORAL ALL GAMES
    {
        GameData gameData = new GameData(AssetLocator.ALLGAMES, pointsToWin, lastPlaceElim, lastPlacePenalty);
        GameManager.Instance.AssignGameData(gameData);
    }

    public void ChangeGameMode(bool right)
    {
        int maxLenght = Enum.GetValues(typeof(GameData.GameMode)).Length-1;

        if (right) gameModeIndex++;
        else gameModeIndex--;
        if (gameModeIndex < -1) gameModeIndex = maxLenght;
        if (gameModeIndex > maxLenght) gameModeIndex = -1;

        if (gameModeIndex == -1) gamemodeText.text = "All Games";
        else
        {
            GameModeDisplay.TryGetValue((GameData.GameMode)gameModeIndex, out string val);
            gamemodeText.text = val;
        }   
    }
    public void ChangePointsToWin(bool right)
    {
        if (right) pointsToWin++;
        else pointsToWin--;
        if (pointsToWin < 5) pointsToWin = 15;
        if (pointsToWin > 15) pointsToWin = 5;
        pointsText.text = pointsToWin.ToString();
    }
    public void ChangeElimination()
    {
        lastPlaceElim = !lastPlaceElim;
        eliminationText.text = lastPlaceElim ? "Yes" : "No";
    }
    public void ChangePenalty(bool right)
    {
        if(right) lastPlacePenalty++;
        else lastPlacePenalty--;
        if (lastPlacePenalty < 0) lastPlacePenalty = 5;
        if (lastPlacePenalty > 5) lastPlacePenalty = 0;
        penaltyText.text = $"{lastPlacePenalty} Pts.";
    }
    
    //Para Leer enums como Strings, para tema de menus y tal
    public static readonly Dictionary<GameMode, string> GameModeDisplay = new Dictionary<GameMode, string>
    {
        { GameMode.DeathMatch, "Deathmatch" },
        { GameMode.Catch, "Cath-Match" },
        { GameMode.OneVSAll, "One VS All" },
        { GameMode.TwoVSTwo, "2 VS 2" }
    };
}

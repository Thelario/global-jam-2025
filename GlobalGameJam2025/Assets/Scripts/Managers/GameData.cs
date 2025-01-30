using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[Serializable]
public class GameData
{
    public enum GameMode
    {
        DeathMatch = 0,
        Catch = 1,
        OneVSAll = 2,
        TwoVSTwo = 3
    }
    
    //Cuando se comienza a jugar, todos los jugadores conectados en ese momento
    //en GameManager se asignan aqui. De esta forma, si se desconecta un mando temporalmente
    //se puede unir otra vez al comienzo de la ronda sin perder progreso
    //(por el puto mando de Nacho que va a tirones por ejemplo)
    public List<PlayerData> GamePlayers { get; private set; }
    public int MinigamesPlayed { get; private set; }
    public List<MinigameData> UpcomingMinigames { get; private set; }
    public Dictionary<PlayerData, int> PlayerRankings { get; private set; } // Key: PlayerData, Value: Puntuacion total

    //Penalties
    public int PointsToWin { get; private set; } = 10;
    public bool LastPlaceElimination { get; private set; } = false;
    public int LastPlacePenalty { get; private set; } = 0; // Default penalty


    public GameData(List<MinigameData> allGames, int pointsToWin, bool lastPlaceElim = false, int lastPlacePenal = 0)
    {
        UpcomingMinigames = new List<MinigameData>(allGames);
        PointsToWin = pointsToWin;
        LastPlaceElimination = lastPlaceElim;
        LastPlacePenalty = lastPlacePenal;

        MinigamesPlayed = 0;
        PlayerRankings = new Dictionary<PlayerData, int>();
    }
    public void SetPlayers(List<PlayerData> allPlayers)
    {
        GamePlayers = allPlayers;
    }

    /// <summary>
    /// Mete minijuego en la lista de Minijuegos a jugar
    /// </summary>
    public void AddMinigame(MinigameData minigameData)
    {
        if (minigameData != null)
        {
            UpcomingMinigames.Add(minigameData);
        }
    }

    /// <summary>
    /// Retrieves and removes the next minigame from the list.
    /// </summary>
    public MinigameData GetNextMinigame()
    {
        if (UpcomingMinigames.Count == 0)
            return null;

        MinigameData nextMinigame = UpcomingMinigames[0];
        UpcomingMinigames.RemoveAt(0);
        MinigamesPlayed++;
        return nextMinigame;
    }

    /// <summary>
    /// Modifica Ranking de un jugador (siempre add)
    /// </summary>
    public void SetPlayerRanking(PlayerData playerData, int rank)
    {
        if (playerData != null && rank > 0)
        {
            PlayerRankings[playerData] += rank;
        }
    }

    /// <summary>
    /// Devuelve Ranking jugador (si existe o null)
    /// </summary>
    public int? GetPlayerRanking(PlayerData playerData)
    {
        return PlayerRankings.ContainsKey(playerData) ? PlayerRankings[playerData] : (int?)null;
    }

    public void ApplyLastPlacePenalty()
    {
        if (PlayerRankings.Count == 0) return;

        // Find the lowest score
        int minScore = PlayerRankings.Values.Min();

        // Apply penalty to all players who have the lowest score
        foreach (var player in PlayerRankings.Keys.ToList())
        {
            if (PlayerRankings[player] == minScore)
            {
                PlayerRankings[player] -= LastPlacePenalty;
            }
        }
    }

    /// <summary>
    /// Reset Todo
    /// </summary>
    public void ResetGameData()
    {
        MinigamesPlayed = 0;
        UpcomingMinigames.Clear();
        PlayerRankings.Clear();
    }
}

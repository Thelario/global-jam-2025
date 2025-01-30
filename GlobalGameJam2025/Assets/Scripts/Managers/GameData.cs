using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GameData
{
    public int MinigamesPlayed { get; private set; }
    public List<MinigameData> UpcomingMinigames { get; private set; }
    public Dictionary<PlayerData, int> PlayerRankings { get; private set; } // Key: PlayerData, Value: Puntuacion total

    public GameData()
    {
        MinigamesPlayed = 0;
        UpcomingMinigames = new List<MinigameData>();
        PlayerRankings = new Dictionary<PlayerData, int>();
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

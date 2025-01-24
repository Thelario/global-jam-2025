using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// MINIGAME MANAGER: Al comienzo, busca 
/// </summary>
public class MinigameManager : Singleton<MinigameManager>
{
    // Events
    public event UnityAction OnMinigameInit;
    public event UnityAction OnMinigameStart;
    public event UnityAction OnMinigameEnd;

    #region Assign Minigame
    //Diccionario cargado por Lazy, carga todos los minijuegos en Resources
    private static Lazy<Dictionary<MinigameBase, string>> m_GamesLoaded = new Lazy<Dictionary<MinigameBase, string>>(() =>
    {
        var gameList = new Dictionary<MinigameBase, string>();
        var minigameFiles = Resources.LoadAll<MinigameBase>("");
        foreach (var minigame in minigameFiles) gameList.Add(minigame, minigame.Name());
        return gameList;
    });
    public static Dictionary<MinigameBase, string> GamesLoaded() => m_GamesLoaded.Value;

    
    [SerializeField] private MinigameBase TESTING_GAME;//TODO:SOLO TESTEO


    public static void AssignMinigamesRandom()
    {
        if (m_GamesLoaded.Value.Count < 4) return;
        
        var availableMinigames = new List<MinigameBase>(m_GamesLoaded.Value.Keys);
        for (int i = availableMinigames.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            var temp = availableMinigames[i];
            availableMinigames[i] = availableMinigames[randomIndex];
            availableMinigames[randomIndex] = temp;
        }

        m_GameList = availableMinigames.GetRange(0, 4);
    }
    #endregion
    
    private static List<MinigameBase> m_GameList;//Todos los juegos que se van a jugar
    private MinigameBase m_currentMinigame;

    protected override void Awake()
    {
        base.Awake();
        //TODO: Remove on build
        if (TESTING_GAME) m_GameList = new List<MinigameBase>() { TESTING_GAME };
        InitMinigame();
    }

    public void InitMinigame()
    {
        if (m_currentMinigame)
        {
            //Terminar el current, asignar y quitarlo de la lista. Inicializarlo y empezar
            EndMinigame();
            m_currentMinigame = m_GameList[0];
            m_GameList.RemoveAt(0);

            OnMinigameInit?.Invoke();
            m_currentMinigame.MinigameInit();
            StartMinigame();
        }
    }

    public void StartMinigame()
    {
        if (m_currentMinigame)
        {
            OnMinigameStart?.Invoke();
            m_currentMinigame.MinigameStart();
        }
    }

    private void Update()
    {
        if (m_currentMinigame) m_currentMinigame.MinigameUpdate();
    }

    public void EndMinigame()
    {
        if (m_currentMinigame)
        {
            OnMinigameEnd?.Invoke();
            m_currentMinigame.MinigameEnd();
            Debug.Log("Minigame Ended: " + m_currentMinigame.name);
            m_currentMinigame = null;
        }
    }  
}

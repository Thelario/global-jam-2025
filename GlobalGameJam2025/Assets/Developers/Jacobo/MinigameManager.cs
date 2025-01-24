using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// MINIGAME MANAGER: Al comienzo, busca 
/// </summary>
public class MinigameManager : MonoBehaviour
{
    // Events
    public event UnityAction OnMinigameInit;
    public event UnityAction OnMinigameStart;
    public event UnityAction OnMinigameEnd;

    #region Assign Minigame
    [SerializeField] private MinigameBase TESTING_GAME;//TODO:SOLO TESTEO
    
    private MinigameBase m_currentMinigame;//Referencia al juego en runtime
    public void AssignMinigame(MinigameBase newMinigame)
    {
        if(newMinigame) m_currentMinigame = newMinigame;
    }
    #endregion
    
    private void Awake() => InitMinigame();

    public void InitMinigame()
    {
        if (m_currentMinigame) EndMinigame();
        if (TESTING_GAME) m_currentMinigame = TESTING_GAME;
        if (m_currentMinigame)
        {
            OnMinigameInit?.Invoke();
            m_currentMinigame.MinigameInit();//Instanciar Prefabs del minijuego
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

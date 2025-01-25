using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

/// <summary>
/// PlayerData se encarga de llevar la info de cada jugador
/// conectado. El input que usa y el numero de puntos que tiene.
/// Se deberia crear cuando se conecte un jugador, y se mantiene hasta
/// que se termine la partida y gane alguien
/// </summary>
[System.Serializable]
public enum SkinType
{
    Default = 0,
    Red = 1,
    Blue = 2,
    Green = 3
}
public class PlayerData
{
    public PlayerData(int newIndex, InputDevice newDevice, SkinType newSkinType = SkinType.Default)
    {
        this.m_index = newIndex;
        this.m_device = newDevice;
        this.m_SkinType = newSkinType;
        this.m_totalPoints = 0;
    }
    private int m_index;
    public int Index => m_index;

    private SkinType m_SkinType;
    private SkinType SkinType => m_SkinType;
    //Getters
    private InputDevice m_device;
    public InputDevice Device => m_device;
    
    private int m_totalPoints;
    public int TotalPoints => m_totalPoints;
}

public class GameManager : Singleton<GameManager>
{
    const int MAX_PLAYER = 4;

    private List<PlayerData> playerData;
    public List<PlayerData> GetAllPlayer() => playerData;
    public int NumberOfPlayers() => playerData.Count;
    public event UnityAction OnPlayerAdded;
    public event UnityAction OnPlayerRemoved;
    
    public void ChangeSkin(PlayerData pl, bool nextSkin = true)
    {
        if (!playerData.Contains(pl)) return;
        Debug.Log($"Next skin assigned for Player{pl.Index}");
    }
    public void AddPlayer(InputDevice id)
    {
        PlayerData newPlayerData = new PlayerData(playerData.Count, id);
        playerData.Add(newPlayerData);
        OnPlayerAdded?.Invoke();
    }
    public void RemovePlayer(InputDevice id)
    {
        PlayerData newPlayerData = new PlayerData(playerData.Count, id);
        playerData.Remove(newPlayerData);
        OnPlayerRemoved?.Invoke();
        ReassignPlayerIndices();
    }

    private void ReassignPlayerIndices()
    {
        
    }


    #region CambiarTransition
    // TRANSITION STUFF
    [Header("Transition Parameters")]
    [SerializeField] RectTransform left;
    [SerializeField] RectTransform right;
    [SerializeField] TMP_Text level_Text;
    [SerializeField] TMP_Text levelNum_Text;
    [SerializeField] Image icon;
    float transitionDuration = 1;

    float openValue = 700;
    float closedValue = 210;

    // Devuelve true si se esta en medio de una transicion
    [HideInInspector] public bool duringTransition = false;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        //INIT
        playerData = new List<PlayerData>();
       
        // Todos los elementos de la transicion empiezan inactivos
        //level_Text.gameObject.SetActive(false);
        //levelNum_Text.gameObject.SetActive(false);
        //icon.gameObject.SetActive(false);
    }

    // Se llama cuando se han elegido todos los jugadores y se cambia de escena
    public void AllPlayersSelected(CharacterData[] charactersData, InputDevice[] devices)
    {
        Debug.Log("ALL PLAYERS SELECTED");
        //currentCharactersData = charactersData;
        //currentDevices = devices;

        //for (int i = 0; i < currentCharactersData.Length; i++)
        //{
        //    if (currentCharactersData[i] != null)
        //        Debug.Log("currentCharactersData_" + i + " = " + currentCharactersData[i].name 
        //            + " with input = " + devices[i]);
        //}

        //ChangeScene("Gameplay");
        //ChangeScene("HexagonalPushes");
    }


    #region Change Scene
    public void ChangeScene(string sceneName)
    {
        if (duringTransition) return;

        StartCoroutine(ChangeSceneCoroutine(-1, sceneName));
        icon.gameObject.SetActive(true);
    }

    public void ChangeScene(int level)
    {
        level_Text.gameObject.SetActive(true);
        levelNum_Text.gameObject.SetActive(true);
        levelNum_Text.text = level.ToString();

        StartCoroutine(ChangeSceneCoroutine(level, ""));
    }

    float transitionsMove;
    // Se encarga de mostrar la transicion y cambiar de escena
    IEnumerator ChangeSceneCoroutine(int level, string sceneName)
    {
        #region Transition animation
        // Close transition
        left.anchoredPosition = new Vector3(-openValue, left.anchoredPosition.y);
        //left.position = new Vector3(leftOpen, left.position.y);
        right.anchoredPosition = new Vector3(openValue, right.anchoredPosition.y);

        DOTween.To(x => transitionsMove = x, openValue, closedValue, transitionDuration)
            .OnUpdate(updateTransitionWalls).SetUpdate(true).SetEase(Ease.OutCirc);

        icon.rectTransform.anchoredPosition = new Vector2(0, 500);
        icon.rectTransform.DOAnchorPosY(0, transitionDuration)
            .SetUpdate(true).SetEase(Ease.OutCirc);
        icon.transform.DORotate(new Vector3(0, 0, 360), transitionDuration, RotateMode.FastBeyond360)
            .SetUpdate(true).SetEase(Ease.OutBack);
        //.SetUpdate(true).SetEase(Ease.OutSine);

        duringTransition = true;
        yield return new WaitForSecondsRealtime(transitionDuration + .5f);

        DOTween.To(x => transitionsMove = x, closedValue, openValue, transitionDuration)
            .OnUpdate(updateTransitionWalls).SetUpdate(true).SetEase(Ease.InCirc);

        icon.rectTransform.DOAnchorPosY(-500, transitionDuration)
            .SetUpdate(true).SetEase(Ease.InCirc);
        icon.transform.DORotate(new Vector3(0, 0, 360), transitionDuration, RotateMode.FastBeyond360)
            .SetUpdate(true).SetEase(Ease.InBack);
        //.SetUpdate(true).SetEase(Ease.InSine);

        #endregion

        Time.timeScale = 1;

        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene("Level_" + level);
        }

        yield return new WaitForSecondsRealtime(transitionDuration);
        duringTransition = false; // Termina la transicion

        // Poner todo de vuelta a false
        level_Text.gameObject.SetActive(false);
        levelNum_Text.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
    }

    void updateTransitionWalls()
    {
        left.anchoredPosition = new Vector3(-transitionsMove, left.anchoredPosition.y);
        right.anchoredPosition = new Vector3(transitionsMove, right.anchoredPosition.y);
    }

    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    // Se van a�adiendo a los jugadores que van perdiendo
    public void AddLoser(PlayerCharacterController playerController)
    {
        //int loserPlayerIndex = playerController.GetComponent<MultiplayerInstance>().playerIndex;

        //for (int i = winPositions.Length - 1; i >= 0; i--)
        //{
        //    if (winPositions[i] == null)
        //    {
        //        winPositions[i] = currentCharactersData[loserPlayerIndex];
        //        break;
        //    }
        //}
    }
}

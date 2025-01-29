using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [Header("Player Panel Window")]
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private List<PlayerProfileUI> profileList;

    [Space(10)]
    [Header("UI Nav")]
    [SerializeField] private Button gobackButton;
    [SerializeField] private Button continueButton;

    private List<PlayerCore> playerList = new List<PlayerCore>();

    //References
    GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        
        gameManager.OnPlayerAdded += PlayerAdded;
        gameManager.OnPlayerRemoved += PlayerRemoved;

        if (gobackButton) gobackButton.onClick.AddListener(GoToMainMenu);
        if (continueButton) continueButton.onClick.AddListener(TryChangeScene);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerAdded -= PlayerAdded;
        GameManager.Instance.OnPlayerRemoved -= PlayerRemoved;

        if (gobackButton) gobackButton.onClick.RemoveAllListeners();
        if (continueButton) continueButton.onClick.RemoveAllListeners();
    }
    private void GoToMainMenu()
    {
        //Siempre empezar de 0 en PlayerSelect
        //Para evitar que se cree uno nuevo cuando se borran todos,
        //y tenga el creat auto keyboard
        GameManager.Instance.OnPlayerAdded -= PlayerAdded;
        gameManager.ClearAllPlayers();
        SceneNav.GoTo(SceneType.MainMenuScene);
    }
    private void Start()
    {
        StartPlayerUIFX();
        UpdatePlayerUI();
        //Crear players que ya existan 
        foreach(PlayerData pl in gameManager.GetPlayerList()) PlayerAdded(pl);
    }

    private void StartPlayerUIFX()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].transform.DOScale(Vector3.one * 1.025f, 0.3f).SetDelay(i * Random.Range(0.2f, 0.7f))
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
    private void UpdatePlayerUI()
    {
        foreach (var cg in profileList) cg.SetProfileEmpty();

        List<PlayerData> allPlayers = GameManager.Instance.GetPlayerList();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            profileList[i].SetProfile(1.0f, allPlayers[i]);
        }
        playerCountText.text = $"Players {allPlayers.Count}/{GameSettings.MAX_PLAYERS}";
    }

    private void TryChangeScene()
    {
        if (GameManager.Instance.PlayerCount < 1) return;
        SceneNav.GoTo(SceneType.GameSettings);
    }
   
    private void PlayerAdded(PlayerData newPlayer)
    {
        Vector3 spawnPos = Random.insideUnitSphere * 3;
        spawnPos.y = 1;
        PlayerCore core = Instantiate(AssetLocator.PlayerPrefab, spawnPos, Quaternion.identity);
        core.InitPlayer(newPlayer);
        playerList.Add(core);
        UpdatePlayerUI();
    }

    private void PlayerRemoved(PlayerData newPlayer)
    {
        PlayerCore playerToRemove = playerList.Find(p => p.PlayerData == newPlayer);
        if (playerToRemove != null)
        {
            playerList.Remove(playerToRemove);
            Destroy(playerToRemove.gameObject);
        }
        UpdatePlayerUI();
    }
}

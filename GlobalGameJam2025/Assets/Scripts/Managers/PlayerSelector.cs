using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    EventBinding<PlayerConnectionEvent> OnPlayerConnected;

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
        UIPanel.GetPanel(typeof(RosterPanel)).Show();

        //Added
        OnPlayerConnected = new EventBinding<PlayerConnectionEvent>(HandlePlayerConnection);
        EventBus<PlayerConnectionEvent>.Register(OnPlayerConnected);

        if (gobackButton) gobackButton.onClick.AddListener(GoToMainMenu);
        if (continueButton) continueButton.onClick.AddListener(TryChangeScene);
    }

    private void OnDisable()
    {
        EventBus<PlayerConnectionEvent>.DeRegister(OnPlayerConnected);

        if (gobackButton) gobackButton.onClick.RemoveAllListeners();
        if (continueButton) continueButton.onClick.RemoveAllListeners();
    }
    private void GoToMainMenu()
    {
        //Siempre empezar de 0 en PlayerSelect
        //Para evitar que se cree uno nuevo cuando se borran todos,
        //y tenga el creat auto keyboard
        //GameManager.Instance.OnPlayerAdded -= PlayerAdded;
        gameManager.ClearAllPlayers();
        SceneNav.GoTo(SceneType.MainMenuScene);
    }
    private void Start()
    {
        UpdatePlayerUI();
        //Crear players que ya existan 
        foreach(PlayerData pl in gameManager.PlayersConnected) PlayerAdded(pl);
    }

    private void UpdatePlayerUI()
    {
        foreach (var cg in profileList) cg.SetProfileEmpty();

        List<PlayerData> allPlayers = GameManager.Instance.PlayersConnected;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            profileList[i].SetProfile(1.0f, allPlayers[i]);
        }
        playerCountText.text = $"Players {allPlayers.Count}/{GameSettings.MAX_PLAYERS}";
    }

    private void TryChangeScene()
    {
        if (GameManager.Instance.PlayerCount < 1) return;
        UIPanel.GetPanel(typeof(ConfigPanel)).Show();
    }
   
    private void HandlePlayerConnection(PlayerConnectionEvent pl)
    {
        if(pl.conType == ConnectionType.Connected) PlayerAdded(pl.data);
        else PlayerRemoved(pl.data);
    }
    
    private void PlayerAdded(PlayerData newPlayer)
    {
        Vector3 spawnPos = Random.insideUnitSphere * 5.5f;
        spawnPos.y = 1;
        PlayerCore core = Instantiate(AssetLocator.Data.PlayerPrefab, spawnPos, Quaternion.identity);
        core.InitPlayer(newPlayer, PlayerActionType.Menu);
        core.ToggleMovement(true);

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

using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [Header("Player Panel Window")]
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private List<PlayerProfileUI> playerList;

    [Space(10)]
    [Header("UI Nav")]
    [SerializeField] private Button gobackButton, continueButton;
    

    //References
    GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameManager.OnPlayerAdded += PlayerAdded;
        gameManager.OnPlayerRemoved += PlayerRemoved;

        if (gobackButton) gobackButton.onClick.AddListener(()=> SceneNav.GoTo(SceneType.MainMenuScene));
        if (continueButton) continueButton.onClick.AddListener(TryChangeScene);
    }
    private void Start()
    {
        StartPlayerUIFX();
        UpdatePlayerUI();
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
        foreach (var cg in playerList) cg.SetProfile(0.2f);

        List<PlayerData> allPlayers = GameManager.Instance.GetPlayerList();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            playerList[i].SetProfile(1.0f, allPlayers[i]);
        }
        playerCountText.text = $"Players {allPlayers.Count}/{GameSettings.MAX_PLAYERS}";
    }

    private void TryChangeScene()
    {
        if (GameManager.Instance.PlayerCount < 1) return;
        SceneNav.GoTo(SceneType.GameSettings);
    }
    private void OnDisable()
    {
        GameManager.Instance.OnPlayerAdded -= PlayerAdded;
        GameManager.Instance.OnPlayerRemoved -= PlayerRemoved;
    }
    private void PlayerAdded(PlayerData newPlayer)
    {
        Vector3 spawnPos = Random.insideUnitSphere * 3;
        spawnPos.y = 1;
        Instantiate(AssetLocator.PlayerPrefab, spawnPos, Quaternion.identity);
        UpdatePlayerUI();
    }
    private void PlayerRemoved(PlayerData newPlayer)
    {
        UpdatePlayerUI();
    }
}

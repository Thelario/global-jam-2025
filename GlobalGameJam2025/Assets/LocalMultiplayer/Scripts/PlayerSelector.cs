using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [Header("Player Panel Window")]
    [SerializeField] private TextMeshProUGUI numberPlayers;
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
    }
    private void PlayerRemoved(PlayerData newPlayer)
    {

    }
}

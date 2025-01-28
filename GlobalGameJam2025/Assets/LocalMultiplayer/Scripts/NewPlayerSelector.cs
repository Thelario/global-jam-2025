using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerSelector : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private List<GameObject> playerVisuals;
    [SerializeField] private List<Renderer> playerRenderers;
    [SerializeField] private Button gobackButton, continueButton; 

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerAdded += UpdateUI;
        GameManager.Instance.OnPlayerRemoved += UpdateUI;
        if (continueButton) gobackButton.onClick.AddListener(()=> SceneNav.GoTo(SceneType.MainMenuScene));
        if (continueButton) continueButton.onClick.AddListener(TryChangeScene);
    }
    private void TryChangeScene()
    {
        if (GameManager.Instance.PlayerCount < 1) return;
        SceneNav.GoTo(SceneType.GameSettings);
    }
    private void OnDisable()
    {
        GameManager.Instance.OnPlayerAdded -= UpdateUI;
        GameManager.Instance.OnPlayerRemoved -= UpdateUI;
    }
    private void Awake()
    {
        foreach (var player in playerVisuals) { player.SetActive(false); }
    }
    private void UpdateUI(PlayerData newPlayer = null)
    {
        // Desactivar todos los visuales de los jugadores inicialmente
        foreach (var player in playerVisuals)
        {
            player.SetActive(false);
        }

        int index = GameManager.Instance.PlayerCount;
        //for (int i = 0; i < index && i < playerVisuals.Count; i++)
        //{
        //    playerVisuals[i].SetActive(true);
        //    if(playerVisuals[i].TryGetComponent(out MultiplayerInstance tetas))
        //    {
        //        tetas.AssignData(GameManager.Instance.GetAllPlayer()[i]);
                
        //    } 
        //}
    }

}

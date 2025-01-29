using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private TextMeshProUGUI numberPlayers;
    [SerializeField] private List<PlayerProfileUI> playerList;
    private void OnEnable()
    {
        GameManager.Instance.OnPlayerAdded += UpdateUI;
        GameManager.Instance.OnPlayerRemoved += UpdateUI;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnPlayerAdded -= UpdateUI;
        GameManager.Instance.OnPlayerRemoved -= UpdateUI;
    }
    private void Start()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].transform.DOScale(Vector3.one * 1.025f, 0.3f).SetDelay(i * UnityEngine.Random.Range(0.2f, 0.7f))
                .SetLoops(-1, LoopType.Yoyo);
        }
        UpdateUI();
    }
    private void UpdateUI(PlayerData newPlayer = null)
    {
        foreach(var cg in playerList) cg.SetProfile(0.2f);
        
        List<PlayerData> allPlayers = GameManager.Instance.GetPlayerList();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            playerList[i].SetProfile(1.0f, allPlayers[i]);
        }
        numberPlayers.text = $"Players {allPlayers.Count}/{GameSettings.MAX_PLAYERS}";
    }
    public void RandomSkin(int index)
    {
        List<PlayerData> allPlayers = GameManager.Instance.GetPlayerList();
        PlayerData newData = allPlayers[index];
        newData.SetSkin(PlayerSkin.GetFirstAvailableSkin(allPlayers));
        GameManager.Instance.SetPlayerData(index, newData);
        UpdateUI(newData);
    }
}

using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private TextMeshProUGUI numberPlayers;
    [SerializeField] private List<CanvasGroup> playerList;
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
    private void UpdateUI()
    {
        foreach(var cg in playerList) cg.alpha = 1f;
        
        //Players Connected
        if (!GameManager.Instance) return;
        int connectedPlys = GameManager.Instance.GetAllPlayer().Count;
        numberPlayers.text = $"Players {connectedPlys}/4";
        for (int i = connectedPlys; i < 4; i++)
        {
            playerList[i].alpha = 0.2f;
        }
    }
}

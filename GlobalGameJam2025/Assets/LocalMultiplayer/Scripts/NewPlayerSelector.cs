using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewPlayerSelector : MonoBehaviour
{
    private GameObject playerPrefab;
    [Header("Players")]
    [SerializeField] private List<Vector3> spawnPositions;
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
        playerPrefab = AssetLocator.PlayerPrefab();
    }
    private void UpdateUI(PlayerData newPlayer = null)
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayMultiplayerManager : MonoBehaviour
{
    //[Header("Debug")]
    //[SerializeField] bool spawnPlayersOnStart = true;

    //List<MultiplayerInstance> allPlayers;
    //public List<MultiplayerInstance> GetAllPlayers => allPlayers;
    
    //private GameObject playerPrefab;
    //private void OnEnable()
    //{
    //    MinigameManager.Instance.OnMinigameStart += SpawnPlayers;
    //}
    //private void OnDisable()
    //{
    //    MinigameManager.Instance.OnMinigameStart -= SpawnPlayers;
    //}
    //public void SpawnPlayers()
    //{
    //    playerPrefab = AssetLocator.PlayerPrefab();
    //    allPlayers = new List<MultiplayerInstance>();
        
    //    List<Vector3> newPosList = new List<Vector3>();
    //    List<SpawnPoint> spawnPoints = SpawnPoint.GetSpawnPoints();
    //    foreach (var pos in spawnPoints)
    //    {
    //        newPosList.Add(pos.SpawnPosition);
    //    }

    //    List<PlayerData> allPlayerData = GameManager.Instance.GetAllPlayer();
    //    for (int i = 0; i < allPlayerData.Count-1; i++)
    //    {
    //        Debug.Log("AAAAAAAA");
    //        GameObject gb = Instantiate(playerPrefab, spawnPoints[i].SpawnPosition, Quaternion.identity);
    //        if (gb.TryGetComponent(out MultiplayerInstance multi)) allPlayers.Add(multi);
    //    }
    //}

    //public void SpawnPlayers()
    //{
    //    CharacterData[] characters = GameManager.GetInstance().currentCharactersData;
    //    InputDevice[] devices = GameManager.GetInstance().currentDevices;

    //    allPlayers = new List<MultiplayerInstance>();

    //    for (int i = 0; i < characters.Length; i++)
    //    {
    //        if (characters[i] == null) continue;

    //        // Si no todos los jugadores fuesen el mismo prefab, se podria elegir un
    //        // prefab dependiendo del characterData que haya elegido cada jugador
    //        GameObject characterPrefab = GameManager.GetInstance().playerPref;

    //        MultiplayerInstance newPlayer = PlayerInput.Instantiate(characterPrefab, pairWithDevice: devices[i]).GetComponent<MultiplayerInstance>();
    //        newPlayer.playerIndex = i;
    //        newPlayer.name = "Player_" + newPlayer.playerIndex;
    //        newPlayer.UpdateCharacterGfx(characters[i]);

    //        newPlayer.transform.parent = playerContainer;
    //        newPlayer.transform.position = playerPositions[allPlayers.Count].position;

    //        allPlayers.Add(newPlayer);
    //    }
    //}

    //public List<MultiplayerInstance> GetAllPlayers()
    //{
    //    return allPlayers;
    //}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayMultiplayerManager : Singleton<GameplayMultiplayerManager>
{
    [Header("Debug")]
    [SerializeField] bool spawnPlayersOnStart = true;
    [SerializeField] bool firstPlayerAsGamepad;
    [SerializeField] CharacterData FirstPlayerData_Debug;
    [SerializeField] CharacterData SecondPlayerData_Debug;

    [Header("References")]
    [SerializeField] Transform playerContainer;
    // Tener en cuenta que tiene que haber igual o mas que el numero maximo de jugadores
    [SerializeField] Transform playerPositionsContainer;
    Transform[] playerPositions;

    List<MultiplayerInstance> allPlayers;

    private void Start()
    {
        playerPositions = new Transform[playerPositionsContainer.childCount];
        for (int i = 0; i < playerPositionsContainer.childCount; i++)
            playerPositions[i] = playerPositionsContainer.GetChild(i);

        // Si no hay jugadores registrados, se crean dos para debugear
        if (GameManager.GetInstance().currentCharactersData[0] == null)
        {
            GameManager.GetInstance().currentCharactersData[0] = FirstPlayerData_Debug;
            GameManager.GetInstance().currentCharactersData[1] = SecondPlayerData_Debug;

            //InputDevice player1_input = Keyboard.current.device;
            //InputDevice player2_input = Gamepad.current;
            InputDevice player1_input;
            InputDevice player2_input;

            if (firstPlayerAsGamepad)
            {
                player1_input = Keyboard.current.device;
                player2_input = Gamepad.current;
            }
            else
            {
                player1_input = Gamepad.current;
                player2_input = Keyboard.current.device;
            }

            GameManager.GetInstance().currentDevices[0] = player1_input;

            if (player2_input != null)
                GameManager.GetInstance().currentDevices[1] = player2_input;
        }

        if (spawnPlayersOnStart)
            SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        CharacterData[] characters = GameManager.GetInstance().currentCharactersData;
        InputDevice[] devices = GameManager.GetInstance().currentDevices;

        allPlayers = new List<MultiplayerInstance>();

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null) continue;

            // Si no todos los jugadores fuesen el mismo prefab, se podria elegir un
            // prefab dependiendo del characterData que haya elegido cada jugador
            GameObject characterPrefab = GameManager.GetInstance().playerPref;

            MultiplayerInstance newPlayer = PlayerInput.Instantiate(characterPrefab, pairWithDevice: devices[i]).GetComponent<MultiplayerInstance>();
            newPlayer.playerIndex = i;
            newPlayer.name = "Player_" + newPlayer.playerIndex;
            newPlayer.UpdateCharacterGfx(characters[i]);

            newPlayer.transform.parent = playerContainer;
            newPlayer.transform.position = playerPositions[allPlayers.Count].position;

            allPlayers.Add(newPlayer);
        }
    }

    public List<MultiplayerInstance> GetAllPlayers()
    {
        return allPlayers;
    }
}

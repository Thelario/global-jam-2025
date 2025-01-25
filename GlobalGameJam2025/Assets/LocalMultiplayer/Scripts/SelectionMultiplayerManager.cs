using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class SelectionMultiplayerManager : MonoBehaviour
{
    [SerializeField] Transform playerContainer;

    // Tener en cuenta que tiene que haber igual o mas que el numero maximo de jugadores
    [SerializeField] Transform playerPositionsContainer;
    Transform[] playerPositions;

    // Local multiplayer variables
    PlayerInputManager playerInputManager;
    List<PlayerInput> allPlayers;

    List<Character> charactersBeingUsed;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();

        allPlayers = new List<PlayerInput>();

        charactersBeingUsed = new List<Character>();

        playerPositions = new Transform[playerPositionsContainer.childCount];
        for (int i = 0; i < playerPositionsContainer.childCount; i++)
            playerPositions[i] = playerPositionsContainer.GetChild(i);
    }

    public void PlayerJoined(PlayerInput newplayer)
    {
        GameManager.Instance.AddPlayer(newplayer.devices[0]);
        //// Asignaciones al script del jugador
        //PlayerSelection newPlayerCard = newplayer.GetComponent<PlayerSelection>();
        //newPlayerCard.selectionMultiplayerManager = this;
        //newPlayerCard.playerIndex = playerInputManager.playerCount - 1;
        //
        //newPlayerCard.transform.SetParent(playerContainer);
        //
        //// SpawnAnimation
        //if (allPlayers.Count < playerPositions.Length)
        //    newplayer.transform.position = playerPositions[allPlayers.Count].position;
        //else
        //{
        //    Debug.LogError("Not Enough player positions");
        //    Debug.LogError("Player Index : " + newPlayerCard.playerIndex);
        //    Debug.LogError("playerPositions.Length : " + playerPositions.Length);
        //}
        //
        //newplayer.transform.localScale = Vector3.zero;
        //newplayer.transform.DOScale(1, 2).SetEase(Ease.OutElastic);
        //
        //allPlayers.Add(newplayer);
        //
        //
        //Character characterSelectedForThisPlayer = GetFirstAvailableCharacter();
        //newPlayerCard.characterSelected = GameManager.Instance.GetCharacterData(characterSelectedForThisPlayer);
        //charactersBeingUsed.Add(characterSelectedForThisPlayer);
    }

    private Character GetFirstAvailableCharacter()
    {
        // Recorre todos los valores del enum
        foreach (Character value in Enum.GetValues(typeof(Character)))
        {
            if (!charactersBeingUsed.Contains(value))
                return value;
        }

        // Si no hay disponibles, devuelve null
        return Character.blueCharacter;
    }

    public CharacterData SelectLeftCharacterData(Character currentCharacter)
    {
        return null;
        //GameManager.Instance.ChangeSkin();
    }

    public Character SelectLeftCharacter(Character currentCharacter)
    {
        Character newSelectedCharacter = currentCharacter;

        Character[] characters = (Character[])Enum.GetValues(typeof(Character));
        int currentIndex = Array.IndexOf(characters, currentCharacter);

        // Buscar hacia la derecha desde el índice actual
        for (int i = currentIndex - 1; i >= 0; i--)
        {
            if (!charactersBeingUsed.Contains(characters[i]))
                newSelectedCharacter = characters[i];
        }

        // Si no encuentra, buscar desde el inicio del array
        for (int i = characters.Length - 1; i > currentIndex; i--)
        {
            if (!charactersBeingUsed.Contains(characters[i]))
                newSelectedCharacter = characters[i];
        }

        // Si se ha encontrado un nuevo character disponible
        if (newSelectedCharacter != currentCharacter)
        {
            charactersBeingUsed.Remove(currentCharacter);
            charactersBeingUsed.Add(newSelectedCharacter);
        }

        return newSelectedCharacter; // Si no hay disponibles, devuelve el mismo.
    }

    public CharacterData SelectRightCharacterData(Character currentCharacter)
    {
        return null;
        //return GameManager.Instance.GetCharacterData(SelectRightCharacter(currentCharacter));
    }

    public Character SelectRightCharacter(Character currentCharacter)
    {
        Character newSelectedCharacter = currentCharacter;

        Character[] characters = (Character[])Enum.GetValues(typeof(Character));
        int currentIndex = Array.IndexOf(characters, currentCharacter);

        // Buscar hacia la derecha desde el índice actual
        for (int i = currentIndex + 1; i < characters.Length; i++)
        {
            if (!charactersBeingUsed.Contains(characters[i]))
                newSelectedCharacter = characters[i];
        }

        // Si no encuentra, buscar desde el inicio del array
        for (int i = 0; i <= currentIndex; i++)
        {
            if (!charactersBeingUsed.Contains(characters[i]))
                newSelectedCharacter = characters[i];
        }

        // Si se ha encontrado un nuevo character disponible
        if (newSelectedCharacter != currentCharacter)
        {
            charactersBeingUsed.Remove(currentCharacter);
            charactersBeingUsed.Add(newSelectedCharacter);
        }

        return newSelectedCharacter; // Si no hay disponibles, devuelve el mismo.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EndSelection();

            //GameManager.Character[] characters = new GameManager.Character[4];
            //for (int i = 0; i < allPlayers.Count; i++)
            //    characters[i] = allPlayers[i].GetComponent<PlayerCard>().characterSelected.character;

            //GameManager.GetInstance().AllPlayersSelected(characters);




            //// Almacenar los datos de los personajes en orden
            //CharacterData[] charactersData = new CharacterData[4];
            //for (int i = 0; i < allPlayers.Count; i++)
            //    charactersData[i] = allPlayers[i].GetComponent<PlayerSelection>().characterSelected;

            //// Almacenar los dispositivos que se usan en orden
            //InputDevice[] devices = new InputDevice[4];
            //for (int i = 0; i < allPlayers.Count; i++)
            //    devices[i] = allPlayers[i].GetDevice<InputDevice>();

            //GameManager.GetInstance().AllPlayersSelected(charactersData, devices);
        }
    }

    public void PlayersReadyRefresh()
    {
        // Si al menos hay 2 personas conectadas en modo ready
        if (AllPlayersReady() && allPlayers.Count >= 2)
            EndSelection();
    }

    void EndSelection()
    {
        // Almacenar los datos de los personajes en orden
        CharacterData[] charactersData = new CharacterData[4];
        for (int i = 0; i < allPlayers.Count; i++)
            charactersData[i] = allPlayers[i].GetComponent<PlayerSelection>().characterSelected;

        // Almacenar los dispositivos que se usan en orden
        InputDevice[] devices = new InputDevice[4];
        for (int i = 0; i < allPlayers.Count; i++)
            devices[i] = allPlayers[i].GetDevice<InputDevice>();
        SceneNav.GoTo(SceneType.GameSettings);
    }

    bool AllPlayersReady()
    {
        foreach (PlayerInput thisPlayer in allPlayers)
        {
            PlayerSelection playerSelection = thisPlayer.GetComponent<PlayerSelection>();
            if (!playerSelection.GetIsReady())
                return false;
        }

        return true;
    }
}

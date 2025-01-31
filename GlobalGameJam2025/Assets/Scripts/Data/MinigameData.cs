using UnityEngine;

/// <summary>
/// Para crear un scriptableObject de Minigame, por ahora solo
/// requiere un ID para diferenciarlos (no es necesario de momento)
/// Y el prefab a instanciar
/// </summary>

[CreateAssetMenu(fileName = "Minigame Base", menuName = "Minigame", order = 1)]
public class MinigameData : ScriptableObject
{
    [SerializeField] private string minigameID = "GameID";
    [SerializeField] private GameObject miniGamePrefab;

    public string MinigameID => minigameID;
    public GameObject MiniGamePrefab => miniGamePrefab;

}
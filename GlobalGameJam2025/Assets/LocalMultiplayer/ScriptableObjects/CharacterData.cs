using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    public Character character;
    //public string characterName;
    public Material characterMaterial;
}
[System.Serializable]
public enum Character { blueCharacter, redCharacter, greenCharacter, purpleCharacter }

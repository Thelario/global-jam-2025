using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    public Character character;
    //public string characterName;
    public Material characterMaterial;
}

public enum Character { blueCharacter, redCharacter, greenCharacter, purpleCharacter }

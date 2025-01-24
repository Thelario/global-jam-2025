using UnityEngine;

public class MultiplayerInstance : MonoBehaviour
{
    [HideInInspector] public int playerIndex;

    [SerializeField] MeshRenderer characterMesh;

    public void UpdateCharacterGfx(CharacterData characterData)
    {
        characterMesh.material = characterData.characterMaterial;
    }
}

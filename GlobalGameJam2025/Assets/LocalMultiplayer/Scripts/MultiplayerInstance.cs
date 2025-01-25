using UnityEngine;

public class MultiplayerInstance : MonoBehaviour
{
    private PlayerData m_playerData;
    public PlayerData PlayerData => m_playerData;
    public void AssignData(PlayerData data)
    {
        m_playerData = data;
    }
    [HideInInspector] public int playerIndex;

    [SerializeField] MeshRenderer characterMesh;

    public void UpdateCharacterGfx(CharacterData characterData)
    {
        characterMesh.material = characterData.characterMaterial;
    }
}

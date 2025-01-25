using UnityEngine;

public class MultiplayerInstance : MonoBehaviour
{
    private PlayerData m_playerData;
    public PlayerData PlayerData => m_playerData;
    public void AssignData(PlayerData data)
    {
        m_playerData = data;
        if(TryGetComponent(out CollisionPainter painter))
        {
            painter.paintColor = AssetLocator.PaintColors[m_playerData.Index];
        }
    }
    [HideInInspector] public int playerIndex;

    [SerializeField] MeshRenderer characterMesh;

    public void UpdateCharacterGfx(CharacterData characterData)
    {
        characterMesh.material = characterData.characterMaterial;
    }
}

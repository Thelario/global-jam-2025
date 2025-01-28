using UnityEngine;

public class MultiplayerInstance : MonoBehaviour
{
    [SerializeField] private Renderer ren;
    [SerializeField] private ParticleSystem part;
    private PlayerData m_playerData;
    public PlayerData PlayerData => m_playerData;
    public void AssignData(PlayerData data)
    {
    //    m_playerData = data;
    //    if(TryGetComponent(out CollisionPainter painter))
    //    {
    //        painter.paintColor = AssetLocator.PaintColors[m_playerData.Index];
    //    }
    //    if (ren != null)
    //    {
    //        Vector2 newOffset = Vector2.zero;
    //        newOffset.x = data.Index * 0.25f;
    //        ren.material.SetTextureOffset("_BaseMap", newOffset);
    //    }
    //    if (part != null)
    //    {
    //        Renderer particleRenderer = part.GetComponent<Renderer>();
    //        if (particleRenderer != null)
    //        {
    //            Material material = particleRenderer.material;
    //            material.SetColor("_BaseColor", AssetLocator.PaintColors[data.Index]);
    //        }
    //    }
    }
    [HideInInspector] public int playerIndex;

    [SerializeField] MeshRenderer characterMesh;

    public void UpdateCharacterGfx(CharacterData characterData)
    {
        characterMesh.material = characterData.characterMaterial;
    }
}

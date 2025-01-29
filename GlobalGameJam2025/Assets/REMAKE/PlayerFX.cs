using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Renderer playerIndicator;
    [SerializeField] private Renderer playerParticles;
    [SerializeField] private Renderer playerDash;

    [Space(10)]
    [Header("Extra")]
    [SerializeField] private CollisionPainter collisionPainter;

    public void Init(PlayerData data)
    {
        int index = GameManager.Instance.GetPlayerIndex(data);
        Color mainColor = data.GetSkin().mainColor;

        if (collisionPainter != null)
        {
            collisionPainter.paintColor = AssetLocator.PaintColors[index];
        }
        if (playerRenderer != null) //Assign Model Color
        {
            Vector2 newOffset = Vector2.zero;
            newOffset.x = index * 0.25f;
            playerRenderer.material.SetTextureOffset("_BaseMap", newOffset);
        }
        if (playerDash != null) //Dash Color
        {
            playerRenderer.material.SetColor("_BaseMap", mainColor);
        }
        if (playerIndicator != null)//Player Indicator Offset
        {
            Vector2 newOffset = Vector2.zero;
            newOffset.x = (index / 8f);
            playerIndicator.material.SetVector("_Offset", newOffset);
        }
        if (playerParticles != null) //Particles Color
        {
            Material material = playerParticles.material;
            material.SetColor("_BaseColor", AssetLocator.PaintColors[index]);
        }
    }
}

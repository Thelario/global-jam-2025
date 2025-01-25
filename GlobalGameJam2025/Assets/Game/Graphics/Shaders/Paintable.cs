using UnityEngine;

public class Paintable : MonoBehaviour
{
    const int TEXTURE_SIZE = 1024;

    public float extendsIslandOffset = 1;

    RenderTexture extendIslandsRenderTexture;
    RenderTexture uvIslandsRenderTexture;
    RenderTexture maskRenderTexture;
    RenderTexture supportTexture;

    Renderer rend;

    int maskTextureID = Shader.PropertyToID("_MaskTexture");

    public RenderTexture getMask() => maskRenderTexture;
    public RenderTexture getUVIslands() => uvIslandsRenderTexture;
    public RenderTexture getExtend() => extendIslandsRenderTexture;
    public RenderTexture getSupport() => supportTexture;
    public Renderer getRenderer() => rend;

    void Start()
    {
        maskRenderTexture = CreateRenderTexture();
        extendIslandsRenderTexture = CreateRenderTexture();
        uvIslandsRenderTexture = CreateRenderTexture();
        supportTexture = CreateRenderTexture();

        rend = GetComponent<Renderer>();
        rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);

        if(PaintManager.Instance != null) PaintManager.Instance.initTextures(this);
    }

    RenderTexture CreateRenderTexture()
    {
        var renderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.useMipMap = false;
        renderTexture.autoGenerateMips = false;
        return renderTexture;
    }

    void OnDisable()
    {
        maskRenderTexture.Release();
        uvIslandsRenderTexture.Release();
        extendIslandsRenderTexture.Release();
        supportTexture.Release();
    }
}

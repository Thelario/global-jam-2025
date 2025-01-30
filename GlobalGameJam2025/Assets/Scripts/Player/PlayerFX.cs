using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField] private PlayerFollow playerFollow;
    [Header("Renderers")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Renderer playerIndicator;
    [SerializeField] private Renderer playerParticles;
    [SerializeField] private Renderer playerDash;

    [Space(10)]
    [Header("Extra")]
    [SerializeField] private CollisionPainter collisionPainter;
    private Sequence spawnSeq;

    [Header("Collision FX")]
    [SerializeField] private float scaleMultiplier = 0.85f;
    Sequence collSeq;

    public void Init(PlayerData data)
    {
        playerFollow.Init(this as MonoBehaviour);
        RefreshRenderer(data);
        PlaySpawnAnim();
        //TODO: Cambiar esto por un wrapper en Core, Definir una vez la Sequence
        GetComponent<PlayerController>().onPlayerCollision += CollisionFX;
    }

    private void CollisionFX()
    {
        if (collSeq != null) collSeq.Restart();
        else collSeq = DOTween.Sequence();
        collSeq.Append(playerRenderer.transform.DOScale(scaleMultiplier, 0.15f)
            .SetLoops(2, LoopType.Yoyo))
            .Join(playerRenderer.material.DOFloat(0.75f, "_Sat", 0.075f).SetLoops(2, LoopType.Yoyo));
    }

    private void OnDestroy()
    {
        spawnSeq?.Kill();
        collSeq?.Kill();
    }
    private void PlaySpawnAnim()
    {
        if(spawnSeq != null) spawnSeq.Restart();
        else spawnSeq = DOTween.Sequence();

        Transform playerTr = playerRenderer.transform;
        playerTr.localScale = Vector3.zero;
        playerRenderer.material.SetFloat("_Sat", 1.0f);

        spawnSeq.Append(playerTr.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
        .Join(playerRenderer.material.DOFloat(0, "_Sat", 0.65f));
    }

    public void RefreshRenderer(PlayerData data)
    {
        int index = GameManager.Instance.GetPlayerIndex(data);
        Color mainColor = data.GetSkin().mainColor;
        if (collisionPainter != null)
        {
            collisionPainter.paintColor = mainColor;
        }
        if (playerRenderer != null) //Assign Model Color
        {
            Vector2 newOffset = Vector2.zero;
            newOffset.x = data.GetSkin().Index * 0.25f;//TODO: NO SE COMO FUNCIONA PERO FUNCIONA. Quitar de AssetLocator
            playerRenderer.material.SetVector("_Offset", newOffset);
        }
        if (playerDash != null) //Dash Color
        {
            playerDash.material.SetColor("_MainColor", mainColor);
        }
        if (playerIndicator != null)//Player Indicator Offset
        {
            Vector2 newOffset = Vector2.zero;
            newOffset.x = (index / 8f);
            playerIndicator.material.SetVector("_Offset", newOffset);
            playerIndicator.material.SetVector("_Color", mainColor);
        }
        if (playerParticles != null) //Particles Color
        {
            Material material = playerParticles.material;
            material.SetColor("_BaseColor", mainColor);
        }
    }
}

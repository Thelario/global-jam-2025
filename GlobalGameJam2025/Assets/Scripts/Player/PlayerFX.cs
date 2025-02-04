using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    EventBinding<PlayerAddedEvent> OnPlayerAdded;
    EventBinding<PlayerRemovedEvent> OnPlayerRemoved;

    [SerializeField] private PlayerFollow playerFollow;
    [Header("Renderers")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Renderer playerIndicator;
    [SerializeField] private Renderer playerDash;
    [SerializeField] private ParticleSystem playerParticles;

    [Space(10)]
    [Header("Extra")]
    [SerializeField] private CollisionPainter collisionPainter;
    private Sequence spawnSeq;

    [Header("Collision FX")]
    [SerializeField] private float scaleMultiplier = 0.85f;

    private PlayerData playerData;
    private Sequence playerSeq;


    #region Init Methods / Main 
    public void Init(PlayerData data)
    {
        playerData = data;
        if (playerFollow) playerFollow.Init(this as MonoBehaviour);
        RefreshRenderer();
        PlaySpawnAnim();
    }
    private void OnEnable() 
    {
        //Events
        OnPlayerAdded = new EventBinding<PlayerAddedEvent>(PlayerReconnected);
        EventBus<PlayerAddedEvent>.Register(OnPlayerAdded);
        OnPlayerRemoved = new EventBinding<PlayerRemovedEvent>(PlayerDisconnected);
        EventBus<PlayerRemovedEvent>.Register(OnPlayerRemoved);

        //TODO: Cambiar esto por un wrapper en Core, Definir una vez la Sequence
        GetComponent<PlayerController>().onPlayerCollision += CollisionFX;
    }
    private void OnDisable()
    {
        spawnSeq?.Kill();
        playerSeq?.Kill();

        EventBus<PlayerAddedEvent>.DeRegister(OnPlayerAdded);
        EventBus<PlayerRemovedEvent>.DeRegister(OnPlayerRemoved);

        GetComponent<PlayerController>().onPlayerCollision -= CollisionFX;
    }
    public void RefreshRenderer()
    {
        int index = GameManager.Instance.GetPlayerIndex(playerData);
        Color mainColor = playerData.GetSkin().mainColor;
        if (collisionPainter != null)
        {
            collisionPainter.paintColor = mainColor;
        }
        if (playerRenderer != null) //Assign Model Color
        {
            Vector2 newOffset = Vector2.zero;
            newOffset.x = playerData.GetSkin().Index / 8f;
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
            Material material = playerParticles.GetComponent<Renderer>().material;
            material.SetColor("_BaseColor", mainColor);
        }
    }
    
    #endregion

    public void KillPlayer()
    {
        Destroy(playerFollow.gameObject);
        playerParticles.Stop();

        if (playerSeq == null || !playerSeq.IsActive()) playerSeq = DOTween.Sequence();

        playerSeq.Append(playerRenderer.transform.DOScale(0, 0.15f).SetEase(Ease.InBack))
            .Join(playerRenderer.material.DOFloat(0.75f, "_Sat", 0.075f))
            .OnComplete(()=> Destroy(this.gameObject));
    }

    private void CollisionFX()
    {
        // Only restart the sequence if it's not active or is null
        if (playerSeq == null || !playerSeq.IsActive()) playerSeq = DOTween.Sequence();
        
        playerSeq.Append(playerRenderer.transform.DOScale(scaleMultiplier, 0.125f)
            .SetLoops(2, LoopType.Yoyo))
        .Join(playerRenderer.material.DOFloat(0.5f, "_Sat", 0.125f).SetLoops(2, LoopType.Yoyo));
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

    
    void PlayerDisconnected(PlayerRemovedEvent playerRemov) 
    {
        RefreshRenderer();
        if (playerIndicator == null || playerRemov.data != playerData) return;

        Vector2 newOffset = Vector2.zero;
        newOffset.x = (7f / 8f);//ultimo de spritesheet
        playerIndicator.material.SetVector("_Offset", newOffset);
        playerIndicator.material.SetVector("_Color", playerData.GetSkin().mainColor);
    }
    void PlayerReconnected(PlayerAddedEvent data)
    {
        RefreshRenderer();
        //if (data == null || data != playerData) return;
    }
}

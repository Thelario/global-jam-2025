using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class PlayerFX : MonoBehaviour, IPlayerEvents
{
    EventBinding<PlayerConnectionEvent> OnPlayerConnection;

    [SerializeField] private PlayerFollow playerFollow;
    [Header("Renderers")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Renderer playerIndicator;
    [SerializeField] private Renderer playerDash;
    [SerializeField] private GameObject hatRenderer;
    [SerializeField] private ParticleSystem playerParticles;

    [Space(10)]
    [Header("Extra")]
    [SerializeField] private CollisionPainter collisionPainter;

    [Header("Collision FX")]
    [SerializeField] private float scaleMultiplier = 0.85f;

    private Sequence playerSeq;
    PlayerController controller;
    PlayerData playerData;

    #region Init Methods / Main 
    public void Init(PlayerData data)
    {
        playerData = data;
        if (playerFollow) playerFollow.Init(this as MonoBehaviour);
        controller = GetComponent<PlayerController>();
        //TODO: Cambiar esto por un wrapper en Core, Definir una vez la Sequence
        controller.onPlayerCollision += CollisionFX;
        RefreshRenderer(data);
        PlaySpawnAnim();
    }
    private void OnEnable() 
    {
        //Events
        OnPlayerConnection = new EventBinding<PlayerConnectionEvent>(PlayerConnected);
        EventBus<PlayerConnectionEvent>.Register(OnPlayerConnection);

        
    }
    private void OnDisable()
    {
        playerSeq?.Kill();

        EventBus<PlayerConnectionEvent>.DeRegister(OnPlayerConnection);

        GetComponent<PlayerController>().onPlayerCollision -= CollisionFX;
    }
    public void RefreshRenderer(PlayerData data)
    {
        int index = GameManager.Instance.GetPlayerIndex(data); //TODO
        Color mainColor = data.GetSkin().mainColor;
        if (collisionPainter != null)
        {
            collisionPainter.paintColor = mainColor;
        }
        if (playerRenderer != null) //Assign Model Color
        {
            Vector2 newOffset = Vector2.zero;
            newOffset.x = data.GetSkin().Index / 8f;
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
        if (!hatRenderer || !playerDash || !playerRenderer) return;
        
        if(playerSeq != null) playerSeq.Kill();
        else playerSeq = DOTween.Sequence();

        Transform playerTr = playerRenderer.transform;
        playerTr.localScale = Vector3.zero;
        hatRenderer.transform.localScale = Vector3.zero;
        playerDash.transform.localScale = Vector3.zero;
        playerRenderer.material.SetFloat("_Sat", 1.0f);

        playerSeq.Append(playerTr.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack))
        .Join(playerRenderer.material.DOFloat(0, "_Sat", 0.65f))
        .Insert(0.15f, playerDash.transform.DOScale(2.5f, 0.4f).SetEase(Ease.OutBack))
        .Join(hatRenderer.transform.DOScale(1, 0.25f).SetEase(Ease.OutBack));
    }

    
    void PlayerConnected(PlayerConnectionEvent playerCon) 
    {
        if (playerIndicator == null) return;

        if (playerCon.data != playerData)
        {
            RefreshRenderer(playerData);
            return;
        }
        
        if (playerCon.conType == ConnectionType.Disconnected)
        {
            Vector2 newOffset = Vector2.zero;
            newOffset.x = (7f / 8f);//ultimo de spritesheet
            playerIndicator.material.SetVector("_Offset", newOffset);
            playerIndicator.material.SetVector("_Color", Color.white);
        }
    }
    private void Update()
    {
        if (!playerDash || controller) return;
        playerDash.material.SetFloat("_FillAmmount", controller.DashTimer / controller.DashReloadTime);
    }
    public void OnPlayerDash() { }

    public void OnPlayerSpecial() => RefreshRenderer(playerData);
}

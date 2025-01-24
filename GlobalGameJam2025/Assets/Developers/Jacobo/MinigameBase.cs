using UnityEngine;

public class MinigameBase : ScriptableObject
{
    [SerializeField] protected GameObject minigamePrefab;
    [SerializeField] protected int maxTimer = 999;

    //protected SceneSearch targetScene;
    public virtual void MinigameInit() { }
    public virtual void MinigameStart() { }
    public virtual void MinigameUpdate() { }
    public virtual void MinigameEnd() { }
}
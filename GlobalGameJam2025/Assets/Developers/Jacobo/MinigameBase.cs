using UnityEngine;

public class MinigameBase : ScriptableObject
{
    [SerializeField] protected string minigameName = "Minigame Default";
    public string Name() => minigameName;
    [SerializeField] protected GameObject minigamePrefab;
    [SerializeField] protected int maxTimer = 999;

    //protected SceneSearch targetScene;
    public virtual void MinigameInit() { }
    public virtual void MinigameStart() { }
    public virtual void MinigameUpdate() { }
    public virtual void MinigameEnd() { }
}
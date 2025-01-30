using UnityEngine;

public class MinigameData : ScriptableObject
{
    [SerializeField] protected string minigameName = "Minigame Default";
    public string Name() => minigameName;
    [SerializeField] protected bool usesTimer = false;
    [SerializeField] protected int maxTimer = 999;
    [SerializeField] protected GameObject minigamePrefab;

    //protected SceneSearch targetScene;
    public virtual void MinigameInit() { }
    public virtual void MinigameStart() { }
    public virtual void MinigameUpdate() { }
    public virtual void MinigameEnd() { }
}
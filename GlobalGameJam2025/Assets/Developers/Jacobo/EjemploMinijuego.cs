using UnityEngine;

[CreateAssetMenu(fileName = "Minigame_Example", menuName = "Minigame/EjemploMinijuego", order = 1)]
public class EjemploMinijuego : MinigameBase
{
    GameObject prefabIns;
    
    public override void MinigameInit() 
    {
        if(minigamePrefab) prefabIns = Instantiate(minigamePrefab);
    }
    public override void MinigameStart()
    {
    }
    public override void MinigameUpdate() 
    {
        //if(prefabIns) prefabIns.transform.Rotate(Vector3.up * 3);
    }
    public override void MinigameEnd()
    {
        if (prefabIns) Destroy(prefabIns.gameObject);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Minigame_Example", menuName = "Minigame/EjemploMinijuego", order = 1)]
public class EjemploMinijuego : MinigameBase
{
    GameObject prefabIns;
    
    public override void MinigameInit() 
    {
        Debug.Log("Init");
        if(minigamePrefab) prefabIns = Instantiate(minigamePrefab);
    }
    public override void MinigameStart()
    {
        Debug.Log("Started");
    }
    public override void MinigameUpdate() 
    {
        prefabIns.transform.Rotate(Vector3.up * 3);
    }
    public override void MinigameEnd()
    {
        Debug.Log("Mingame Ended");
        if (prefabIns) Destroy(prefabIns.gameObject);
    }
}

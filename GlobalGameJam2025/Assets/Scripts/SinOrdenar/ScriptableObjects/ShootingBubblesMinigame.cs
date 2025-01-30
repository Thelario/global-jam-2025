using UnityEngine;

[CreateAssetMenu(fileName = "Minigame_Example", menuName = "Minigame/Shooting Bubbles Minigame", order = 1)]
public class ShootingBubblesMinigame : MinigameBase
{
    GameObject prefabIns;
    
    public override void MinigameInit() 
    {
        Debug.Log("Init ShootingBubblesMinigame");

        if (minigamePrefab)
        {
            prefabIns = Instantiate(minigamePrefab);
        }
    }
    public override void MinigameStart()
    {
        Debug.Log("Started ShootingBubblesMinigame");
    }
    
    public override void MinigameUpdate() 
    {
        //prefabIns.transform.Rotate(Vector3.up * 3);
    }
    
    public override void MinigameEnd()
    {
        Debug.Log("Mingame Ended ShootingBubblesMinigame");
        
        if (prefabIns)
            Destroy(prefabIns.gameObject);
    }
}
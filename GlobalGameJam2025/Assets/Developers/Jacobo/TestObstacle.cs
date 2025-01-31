using System.Collections;
using UnityEngine;

public class RotatingFloor : MonoBehaviour, IMinigameEventListener
{
    public float rotMod = 0.5f;
    float rotationSpeed;

    //Cuando comience el juego
    public void OnMinigameStart()
    {
        rotationSpeed = 2.0f;
    }
    //Cuando termine el juego
    public void OnMinigameEnd()
    {
        rotationSpeed = 0.0f;
    }
    //Cuando un jugador muere
    public void OnPlayerDeath(PlayerCore player)
    {
        rotationSpeed += 5.0f;
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0,1,0) * rotationSpeed);
    }
}

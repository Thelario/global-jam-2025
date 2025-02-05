using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotarCosas : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);
    private bool isRotating = false;
    List<PlayerCore> players;

    void Start()
    {

        StartCoroutine(WaitAndRotate());
    }

    private IEnumerator WaitAndRotate()
    {
        players = PlayerCore.AllPlayers;
        foreach (PlayerCore player in players)
        {
            player.transform.localScale *= 1.5f;
        }
        yield return new WaitForSeconds(3f);
        isRotating = true; 
    }

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}

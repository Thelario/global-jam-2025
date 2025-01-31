using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Barquillo : MonoBehaviour, IMinigameEventListener
{
    public float forceToAdd = 300;
    public ForceMode forceM = ForceMode.VelocityChange;
    MinigameManager manager;
    List<PlayerCore> allPlayers;
    List<Rigidbody> allRbs;
    bool shouldPush = false;
    void Start()
    {
        manager = MinigameManager.Instance;
        allPlayers = manager.PlayerList;
        allRbs = new List<Rigidbody>();
        foreach (var tetas in allPlayers)
        {
            allRbs.Add(tetas.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!shouldPush) return;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb) rb.AddForce(Vector3.forward * forceToAdd, forceM);
    }

    public void OnMinigameStart()
    {
        shouldPush = true;
    }

    public void OnMinigameEnd()
    {
        shouldPush = false;
    }
}

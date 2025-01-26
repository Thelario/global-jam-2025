using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Barquillo : MonoBehaviour
{
    public float forceToAdd = 300;
    public ForceMode forceM = ForceMode.VelocityChange;
    MinigameManager manager;
    List<MultiplayerInstance> allPlayers;
    List<Rigidbody> allRbs;
    bool shouldPush = false;
    void Start()
    {
        manager = MinigameManager.Instance;
        allPlayers = manager.GetAllPlayers();
        allRbs = new List<Rigidbody>();
        foreach (var tetas in allPlayers)
        {
            allRbs.Add(tetas.GetComponent<Rigidbody>());
        }
        manager.OnMinigameStart += () => shouldPush = true;
    }
    private void OnDisable()
    {
        manager.OnMinigameStart -= () => shouldPush = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!shouldPush) return;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb) rb.AddForce(Vector3.forward * forceToAdd, forceM);
    }
}

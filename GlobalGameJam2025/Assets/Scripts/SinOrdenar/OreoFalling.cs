using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreoFalling : MonoBehaviour, IMinigameEventListener
{
    private bool running = false;
    public List<Oreo> oreoList;

    public void StartGame()
    {
        if (running) return;
        running = true;
        StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (running)
        {
            List<Oreo> availableOreos = oreoList.FindAll(oreo => oreo.isAvailable);
            if (availableOreos.Count > 0)
            {
                Oreo randomOreo = availableOreos[Random.Range(0, availableOreos.Count)];
                randomOreo.FallOreo(); 
            }
            yield return new WaitForSeconds(Random.Range(2.5f, 3.25f));
        }
    }

    public void OnMinigameStart() => StartGame();
}

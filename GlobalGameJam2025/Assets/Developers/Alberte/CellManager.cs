using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    private bool falling = false;
    public List<Cell> cellList;
    void OnEnable()
    {
        MinigameManager.Instance.OnMinigameStart += StartGame;
    }
    void OnDisable()
    {
        MinigameManager.Instance.OnMinigameStart -= StartGame;
    }
    public void StartGame()
    {
        if (falling) return;
        falling = true;
        StartCoroutine(FallRoutine());
    }
    private IEnumerator FallRoutine()
    {
        while(cellList.Count > 0)
        {
            yield return new WaitForSeconds(Random.Range(1, 3.0f));
            int numberOfTilesFalling = Random.Range(1, 5);
            for(int i = 0; i < numberOfTilesFalling; i++)
            {
                Cell selectedCell = cellList[Random.Range(0, cellList.Count)];
                if (selectedCell)
                {
                    selectedCell.Fall();
                    cellList.Remove(selectedCell);
                }
            }
            yield return new WaitForSeconds(Random.Range(5, 7.0f));
        }
    }
}

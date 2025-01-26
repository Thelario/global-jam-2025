using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CellManager : MonoBehaviour
{
    public static float spacing = 4.25f;
    [MenuItem("CONTEXT/MonoBehaviour/Arrange Children in Grid", false, 0)]
    public static void ArrangeChildrenInGrid()
    {
        // Get the selected GameObject
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogError("No GameObject selected. Please select a GameObject with children.");
            return;
        }

        // Get all child transforms
        Transform[] children = selectedObject.GetComponentsInChildren<Transform>();

        if (children.Length <= 1)
        {
            Debug.LogError("The selected GameObject has no children to arrange.");
            return;
        }

        // Define grid dimensions and spacing
        int rows = 8;
        int columns = 7;

        // Arrange children in grid
        int index = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (index >= children.Length - 1) // Skip the parent itself
                    break;

                Transform child = children[index + 1]; // Start from the first child, skipping the parent
                child.localPosition = new Vector3(x * spacing, 0, y * spacing);
                index++;
            }
        }

        Debug.Log("Children arranged in an 8x7 grid with 0.5 spacing.");
    }
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
            yield return new WaitForSeconds(Random.Range(2, 5.0f));
            int numberOfTilesFalling = Random.Range(2, 6);
            for(int i = 0; i < numberOfTilesFalling; i++)
            {
                Cell selectedCell = cellList[Random.Range(0, cellList.Count)];
                if (selectedCell)
                {
                    selectedCell.Fall();
                    cellList.Remove(selectedCell);
                }
            }
        }
    }
}

using System;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    //References
    private GameObject podiumPrefab;

    private void Start()
    {
        podiumPrefab = AssetLocator.Data.PodiumPrefab;
        if (podiumPrefab) PlacePodiums();
    }
    private void Update()
    {
        //Debug.Log(GameManager.Instance.PlayerCount);
    }
    private void PlacePodiums()
    {
        float spacing = 7f;
        int playerCount = GameManager.Instance.PlayerCount;
        bool isEven = playerCount % 2 == 0;
        int midIndex = playerCount / 2;

        for (int i = 0; i < playerCount; i++)
        {
            GameObject podium = Instantiate(podiumPrefab, transform);
            float offset = (i - midIndex) * spacing + (isEven ? spacing / 2 : 0);
            float zOffset = isEven ? 0 : -1f;
            podium.transform.position = new Vector3(offset, 1, zOffset);
        }
    }
}

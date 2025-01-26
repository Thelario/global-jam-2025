using System.Collections;
using UnityEngine;

public class PlayerPointTower : MonoBehaviour
{
    static public float distanceBetweenChips = 1;

    [SerializeField] Transform chipPrefab;

    float currentHeight;

    [SerializeField] float chipsZXOffset = .5f;

    float spawnHighHeight = 20;

    private void Awake()
    {
        currentHeight = distanceBetweenChips;
    }

    private void Start()
    {
        //for (int i = 0; i < 5; i++)
        //    AddChip(false);
    }

    public void AddChip(bool highHeight)
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * chipsZXOffset;

        Vector3 finalPos;
        if (highHeight)
            finalPos = new Vector3(randomOffset.x, spawnHighHeight, randomOffset.y) + transform.position;
        else
            finalPos = new Vector3(randomOffset.x, currentHeight, randomOffset.y) + transform.position;

        float randomAngle = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);
        Transform newChip = Instantiate(chipPrefab, finalPos, randomRotation, transform).transform;

        currentHeight += distanceBetweenChips;
    }

    public IEnumerator AddChipsDelay(int chips)
    {
        for (int i = 0; i < chips; i++)
        {
            yield return new WaitForSeconds(.3f);
            AddChip(true);
        }
    }

    public Vector3 GetPlayerSpawnPosition()
    {
        return new Vector3(transform.position.x, spawnHighHeight, transform.position.z);
    }
}

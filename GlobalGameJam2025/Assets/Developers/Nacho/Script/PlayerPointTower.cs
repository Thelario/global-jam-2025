using System.Collections;
using UnityEngine;

public class PlayerPointTower : MonoBehaviour
{
    [SerializeField] Transform chipPrefab;

    [SerializeField] float distanceBetweenChips = 1;
    float currentHeight;

    [SerializeField] float chipsZXOffset = .5f;

    float spawnHighHeight = 20;

    private void Awake()
    {
        currentHeight = distanceBetweenChips;
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
            AddChip(false);
    }

    void AddChip(bool highHeight)
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * chipsZXOffset;

        Vector3 finalPos;
        if (highHeight)
            finalPos = new Vector3(randomOffset.x, spawnHighHeight, randomOffset.y) + transform.position;
        else
            finalPos = new Vector3(randomOffset.x, currentHeight, randomOffset.y) + transform.position;

        Instantiate(chipPrefab, finalPos, Quaternion.identity, transform);
        currentHeight += distanceBetweenChips;
    }

    public IEnumerator AddChipsDelay(int chips)
    {
        for (int i = 0; i < chips; i++)
        {
            yield return new WaitForSeconds(1);
            AddChip(true);
        }
    }
}

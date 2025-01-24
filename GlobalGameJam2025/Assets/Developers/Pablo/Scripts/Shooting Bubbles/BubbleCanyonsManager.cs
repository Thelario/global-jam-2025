using UnityEngine;

public class BubbleCanyonsManager : Singleton<BubbleCanyonsManager>
{
    [SerializeField] [Range(0, 1)] private float difficultyMultiplier;
    [SerializeField] private float timeBetweenBubbles;
    [SerializeField] private float minTimeBetweenBubbles;
    [SerializeField] private Transform canyonsParent;

    private int _previousCanyon;
    private float _timeBetweenBubbles;
    private float _timeBetweenBubblesCounter;

    private void Start()
    {
        _previousCanyon = 0;
        _timeBetweenBubbles = timeBetweenBubbles;
        _timeBetweenBubblesCounter = 0f;
    }

    private void Update()
    {
        _timeBetweenBubblesCounter -= Time.deltaTime;
        if (!(_timeBetweenBubblesCounter <= 0f))
            return;

        PickRandomCanyonAndShoot();
        IncreaseDifficulty();
    }

    private void IncreaseDifficulty()
    {
        _timeBetweenBubbles = Mathf.Clamp(_timeBetweenBubbles * difficultyMultiplier, minTimeBetweenBubbles, timeBetweenBubbles);
        _timeBetweenBubblesCounter = _timeBetweenBubbles;
    }

    private void PickRandomCanyonAndShoot()
    {
        int canyon;
        do {
            canyon = Random.Range(0, canyonsParent.childCount);
        }
        while(canyon == _previousCanyon);
        
        _previousCanyon = canyon;
        canyonsParent.GetChild(canyon).GetChild(0).GetComponent<BubbleCanyon>().Shoot();
    }
}

using UnityEngine;

public class NormalRacePosiitions : MonoBehaviour
{
    public GameObject[] players, orderedPlayers;
    public bool ended = false;
    private PlayerCore lastplayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!ended)
            {
                ended = true;
                players = GameObject.FindGameObjectsWithTag("Player");
                float maxZ = float.MinValue;
                while (PlayerCore.AllPlayers.Count > 0)
                {
                    foreach (var player in PlayerCore.AllPlayers)
                    {
                        if (player.transform.position.z > maxZ)
                        {
                            maxZ = player.transform.position.z;
                            lastplayer = player;
                        }
                    }
                    MinigameManager.Instance.PlayerDeath(lastplayer);
                }

            }
        }
        
        
    }
}

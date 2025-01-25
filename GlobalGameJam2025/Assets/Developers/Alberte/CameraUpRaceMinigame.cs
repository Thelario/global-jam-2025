using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraUpRaceMinigame : MonoBehaviour
{
    public Transform secondposition, endposition;
    private bool firstmove, secondmove;
    bool enableinput = false;
    public GameObject[] players;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        StartCoroutine(Begin());
        for(int i=0; i<players.Length; i++)
        {
            if (players[i].GetComponent<PlayerInput>() && players[i].GetComponent<PlayerMovement>())
            players[i].GetComponent<PlayerInput>().enabled = false;
            players[i].GetComponent<PlayerMovement>().enabled = false;
            
        }
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (firstmove)
        {
            transform.position = Vector3.MoveTowards(transform.position, secondposition.position, 5 * Time.deltaTime);
        }
        else if (secondmove)
        {
            transform.position = Vector3.MoveTowards(transform.position, endposition.position, 3 * Time.deltaTime);
        }
    }
    IEnumerator Begin()
    {
        firstmove = true;
        secondmove = false;
        yield return new WaitForSeconds(3);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerInput>() && players[i].GetComponent<PlayerMovement>())
                players[i].GetComponent<PlayerInput>().enabled = true;
            players[i].GetComponent<PlayerMovement>().enabled = true;
        }
        firstmove = false;
        secondmove = true;
    }
}

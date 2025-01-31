using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraUpRaceMinigame : MonoBehaviour
{
    public Transform secondposition, endposition;
    private bool firstmove, secondmove;
    

    void Start()
    {
        StartCoroutine(Begin());
    }

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
        yield return new WaitForSeconds(3f);
        
        firstmove = true;
        secondmove = false;
        
        yield return new WaitForSeconds(1f);
        
        
        
        //for (int i = 0; i < players.Length; i++)
        //{
        //    if (players[i].GetComponent<PlayerInput>() && players[i].GetComponent<PlayerMovement>())
        //        players[i].GetComponent<PlayerInput>().enabled = true;
        //    players[i].GetComponent<PlayerMovement>().enabled = true;
        //}
        firstmove = false;
        secondmove = true;
    }
}

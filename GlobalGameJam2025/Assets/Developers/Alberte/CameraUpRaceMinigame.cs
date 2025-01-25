using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraUpRaceMinigame : MonoBehaviour
{
    public Transform secondposition, endposition;
    private bool firstmove, secondmove;
    bool enableinput = false;
    public Text count;

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
        count.text = "3";
        yield return new WaitForSeconds(1f);
        count.text = "2";
        yield return new WaitForSeconds(1f);
        count.text = "1";
        yield return new WaitForSeconds(1f);
        count.text = "";
        
        
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

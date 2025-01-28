using System.Collections;
using UnityEngine;

public class RotateCrunch : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 0);
    public bool canMove = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Begin());
        if (canMove)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
        
    }
    IEnumerator Begin()
    {
        yield return new WaitForSeconds(3f);
        canMove = true;


    }
}

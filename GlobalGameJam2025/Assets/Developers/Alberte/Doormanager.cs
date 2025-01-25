using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Doormanager : MonoBehaviour
{
    public GameObject doorlevel1;
    public GameObject doorlevel2;
    public GameObject doorlevel3;
    public GameObject doorlevel4;
    public GameObject doorlevel5;
    public Text count;
    public Transform spawnPoint, endPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnDoor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnDoor()
    {
        count.text = "3";
        yield return new WaitForSeconds(1f);
        count.text = "2";
        yield return new WaitForSeconds(1f);
        count.text = "1";
        yield return new WaitForSeconds(1f);
        count.text = "";
        yield return new WaitForSeconds(1f);
        Debug.Log(spawnPoint.position);
        Debug.Log(endPoint.position);
        GameObject doorInstance1=Instantiate(doorlevel1, spawnPoint.transform.position, Quaternion.identity);

        doorInstance1.GetComponent<Rigidbody>().AddForce(doorInstance1.transform.forward * 5 * Time.deltaTime);
        
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Doormanager : MonoBehaviour
{
    public GameObject doorlevel1;
    public GameObject doorlevel2;
    public GameObject doorlevel3;
    public GameObject doorlevel4;
    public GameObject doorlevel5;
    public GameObject doorlevel6, doorlevel7, doorlevel8, doorlevel9, doorlevel10, doorlevel11, doorlevel12, doorlevel13;
    public Text count;
    public Transform spawnPoint, spawnPoint3, spawnPoint4,endPoint;
    public float speed = 30;
    public float increment = 3;
    public float timeWait = 2;
    public float timedecrease = 0.1f;
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
        
        
        GameObject doorInstance1=Instantiate(doorlevel1, spawnPoint.transform.position, Quaternion.identity);
        doorInstance1.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(2f);

        increment *= 1;
        speed += increment;
        GameObject doorInstance2 = Instantiate(doorlevel2, spawnPoint.transform.position, Quaternion.identity);
        doorInstance2.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(2f);

        increment *= 1;
        speed += increment;
        GameObject doorInstance3 = Instantiate(doorlevel3, spawnPoint3.transform.position, Quaternion.identity);
        doorInstance3.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(2f);

        increment *= 1;
        speed += increment;
        GameObject doorInstance4 = Instantiate(doorlevel4, spawnPoint4.transform.position, Quaternion.identity);
        doorInstance4.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(2f);

        increment *= 1;
        speed += increment;
        GameObject doorInstance5 = Instantiate(doorlevel5, spawnPoint3.transform.position, Quaternion.identity);
        doorInstance5.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(2f);

        increment *= 1;
        speed += increment;
        GameObject doorInstance6 = Instantiate(doorlevel6, spawnPoint4.transform.position, Quaternion.identity);
        doorInstance6.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);

        increment *= 1;
        speed += increment;
        GameObject doorInstance7 = Instantiate(doorlevel7, spawnPoint3.transform.position, Quaternion.identity);
        doorInstance7.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);

        increment *= 1;
        speed += increment;
        GameObject doorInstance8 = Instantiate(doorlevel8, spawnPoint4.transform.position, Quaternion.identity);
        doorInstance7.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);
        increment *= 1;
        speed += increment;
        GameObject doorInstance9 = Instantiate(doorlevel9, spawnPoint3.transform.position, Quaternion.identity);
        doorInstance7.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);
        increment *= 1;
        speed += increment;
        GameObject doorInstance10= Instantiate(doorlevel10, spawnPoint4.transform.position, Quaternion.identity);
        doorInstance7.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);
        increment *= 1;
        speed += increment;
        GameObject doorInstance11 = Instantiate(doorlevel11, spawnPoint3.transform.position, Quaternion.identity);
        doorInstance7.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);
        increment *= 1;
        speed += increment;
        GameObject doorInstance12= Instantiate(doorlevel12, spawnPoint4.transform.position, Quaternion.identity);
        doorInstance7.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);
        increment *= 1;
        speed += increment;
        GameObject doorInstance13 = Instantiate(doorlevel13, spawnPoint3.transform.position, Quaternion.identity);
        doorInstance7.GetComponent<Door>().speed = speed;
        yield return new WaitForSeconds(1f);
        




    }
}

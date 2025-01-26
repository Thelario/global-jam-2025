using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate; 
    private int currentIndex = 0; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Triggerable"))
        {
            if (currentIndex < objectsToActivate.Length)
            {
                objectsToActivate[currentIndex].SetActive(true);
                currentIndex++; 
            }
        }
    }
}

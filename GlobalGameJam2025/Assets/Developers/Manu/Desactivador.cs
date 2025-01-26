using UnityEngine;

public class Deactivator : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDeactivate;
    private int currentIndex;

    private void Start()
    {
        currentIndex = objectsToDeactivate.Length - 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Triggerable"))
        {

            if (currentIndex >= 0)
            {
                objectsToDeactivate[currentIndex].SetActive(false);
                currentIndex--;
            }
        }
    }
}

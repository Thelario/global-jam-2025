using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform endPoint;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.TryGetComponent<PlayerController>(out var playerController) != true)
    //        return;

    //    Vector3 hitDirection = collision.transform.position - collision.contacts[0].point;

    //    SoundManager.Instance.PlaySound(Sound.BubbleGiggly);
    //    playerController.SetLinearVelocity(hitDirection * 10);
        
    //}
}

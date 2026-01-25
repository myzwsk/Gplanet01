using UnityEngine;

public class DontSleep : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
    }
}

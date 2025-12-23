using UnityEngine;

public class AOE8Field : MonoBehaviour
{
    private bool isTouchingPlayer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Destroy", 3);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }
    void Destroy()
    {

        if (isTouchingPlayer)
        {
            Debug.Log("プレイヤー死亡！");
        }
        Destroy(gameObject);
    }
}

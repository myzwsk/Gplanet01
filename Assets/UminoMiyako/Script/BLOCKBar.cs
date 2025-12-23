using UnityEngine;

public class BLOCKBar : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤー死亡");
        }
    }
}

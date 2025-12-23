using UnityEngine;

public class aoeDonut2 : MonoBehaviour
{
    public AOEDonut donut;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            donut.isHitCol2 = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            donut.isHitCol2 = false;
    }
}

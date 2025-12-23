using UnityEngine;

public class aoeDonut1 : MonoBehaviour
{
    public AOEDonut donut;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            donut.isHitCol1 = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            donut.isHitCol1 = false;
    }
}

using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    public Transform warpDestination;
    public TeleportUI teleportUI;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("WarpPoint Trigger 入った");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player 判定 OK");
            teleportUI.Open(other.transform, warpDestination);
        }
    }
}

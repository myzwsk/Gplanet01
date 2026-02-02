using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public SceneConfirmUI ui;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ui.Open();
        }
    }
}

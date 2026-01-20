using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    public GameObject miniGameCanvas;
    public SmoothFollowCamera cameraFollow;
    public PlayerRotateToCamera playerRotate;
    public MiniGameManager manager; // ★追加

    bool isTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        // ★ すでに全クリアなら起動しない
        if (manager != null && manager.isAllCleared) return;

        if (isTriggered) return;
        if (!other.CompareTag("Player")) return;

        isTriggered = true;

        miniGameCanvas.SetActive(true);

        if (cameraFollow != null)
            cameraFollow.enableFollow = false;

        if (playerRotate != null)
            playerRotate.enableRotate = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndMiniGame()
    {
        isTriggered = false;

        miniGameCanvas.SetActive(false);

        if (cameraFollow != null)
            cameraFollow.enableFollow = true;

        if (playerRotate != null)
            playerRotate.enableRotate = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    public GameObject miniGameCanvas;
    public SmoothFollowCamera cameraFollow;
    public PlayerRotateToCamera playerRotate;
    public MiniGameManager manager;

    bool isTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // ★ 全クリア後は起動しない（これは正しい）
        if (manager != null && manager.isAllCleared) return;

        // ★ すでに起動中なら無視
        if (isTriggered) return;

        StartMiniGame();
    }

    void StartMiniGame()
    {
        isTriggered = true;

        miniGameCanvas.SetActive(true);

        if (cameraFollow != null)
            cameraFollow.enableFollow = false;

        if (playerRotate != null)
            playerRotate.enableRotate = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("MiniGame Start");
    }

    // ★ 必ず呼ばれる「終了処理」
    public void EndMiniGame()
    {
        isTriggered = false; // ← これが命

        miniGameCanvas.SetActive(false);

        if (cameraFollow != null)
            cameraFollow.enableFollow = true;

        if (playerRotate != null)
            playerRotate.enableRotate = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("MiniGame End");
    }
}

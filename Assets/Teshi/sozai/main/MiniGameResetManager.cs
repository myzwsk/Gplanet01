using UnityEngine;

public class MiniGameResetManager : MonoBehaviour
{
    public static MiniGameResetManager Instance;

    [Header("Panels")]
    public GameObject pressureGamePanel;
    public GameObject finalGamePanel;

    [Header("MiniGame Scripts")]
    public PressureMiniGame pressureMiniGame;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ResetAllMiniGames()
    {
        Debug.Log("=== ミニゲーム完全リセット ===");

        // ① Panel 状態リセット
        if (pressureGamePanel != null)
            pressureGamePanel.SetActive(true);

        if (finalGamePanel != null)
            finalGamePanel.SetActive(false);

        // ② 水圧ゲージ内部リセット
        if (pressureMiniGame != null)
        {
            pressureMiniGame.ResetGame();
        }
        else
        {
            Debug.LogError("PressureMiniGame が設定されていません");
        }
    }
}

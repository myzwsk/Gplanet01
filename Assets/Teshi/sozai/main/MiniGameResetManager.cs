using UnityEngine;

public class MiniGameResetManager : MonoBehaviour
{
    public static MiniGameResetManager Instance;

    [Header("Panels")]
    public GameObject pressureGamePanel;
    public GameObject pipeGamePanel;
    public GameObject finalGamePanel;

    [Header("MiniGame Scripts")]
    public PressureMiniGame pressureMiniGame;
    public PipeGameManager pipeGameManager;

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

        // ① Panel 状態を正解に戻す
        pressureGamePanel.SetActive(true);
        pipeGamePanel.SetActive(false);
        finalGamePanel.SetActive(false);

        // ② 各ミニゲーム内部リセット
        if (pressureMiniGame != null)
            pressureMiniGame.ResetGame();

        if (pipeGameManager != null)
            pipeGameManager.ResetGame();
    }
}

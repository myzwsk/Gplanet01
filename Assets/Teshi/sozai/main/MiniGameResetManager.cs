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

    public void ResetAllMiniGames(bool restart)
    {
        Debug.Log("=== ミニゲームリセット ===");

        if (restart)
        {
            // 再挑戦用
            pressureGamePanel.SetActive(true);
            pressureMiniGame.ResetGame();
        }
        else
        {
            // ★ 中断用（いいえ）
            pressureGamePanel.SetActive(false);
            pressureMiniGame.ForceStop();
        }

        finalGamePanel.SetActive(false);
    }

}

using UnityEngine;

public class PipeGameManager : MonoBehaviour
{
    public PipePiece[] pipes;

    [Header("Next")]
    public GameObject pipeGamePanel;    // MiniGame2Panel
    public GameObject finalGamePanel;   // MiniGame3Panel

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckClear();
        }
    }

    public void CheckClear()
    {
        foreach (var pipe in pipes)
        {
            if (!pipe.IsCorrect())
            {
                Debug.Log("まだつながってない");
                return;
            }
        }

        Debug.Log("配管ミニゲーム クリア！");

        // ★ 配管ミニゲームを消す
        pipeGamePanel.SetActive(false);

        // ★ 最終ミニゲームを出す
        finalGamePanel.SetActive(true);
    }
}

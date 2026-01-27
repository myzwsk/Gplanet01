using UnityEngine;

public class PipeGameManager : MonoBehaviour
{
    public PipePiece[] pipes;

    public GameObject pipeGamePanel;
    public GameObject finalGamePanel;

    void OnEnable()
    {
        ResetGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckClear();
        }
    }

    void CheckClear()
    {
        foreach (var pipe in pipes)
        {
            if (!pipe.IsCorrect())
                return;
        }

        Debug.Log("配管ミニゲーム クリア");

        pipeGamePanel.SetActive(false);
        finalGamePanel.SetActive(true);
    }

    // ★ 追加：完全リセット
    public void ResetGame()
    {
        foreach (var pipe in pipes)
        {
            pipe.RandomizeRotation();
        }

        Debug.Log("PipeGame Reset");
    }
}

using UnityEngine;

public class PressureMiniGame : MonoBehaviour
{
    [Header("UI")]
    public RectTransform greenZone;
    public RectTransform needle;

    [Header("Next MiniGame")]
    public GameObject nextMiniGamePanel;

    int phase = 1;

    float greenMoveSpeed = 0f;
    float needleSpeed = 200f;

    float range = 200f;
    float greenStartX;
    float needleStartX;

    bool greenRight = true;
    bool needleRight = true;
    bool stopped = false;

    void Start()
    {
        greenStartX = greenZone.anchoredPosition.x;
        needleStartX = needle.anchoredPosition.x;
        ResetGame();
    }

    void Update()
    {
        if (!stopped)
        {
            MoveNeedle();
        }

        if (phase >= 2 && !stopped)
        {
            MoveGreenZone();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !stopped)
        {
            StopNeedle();
        }
    }

    void MoveNeedle()
    {
        Vector2 pos = needle.anchoredPosition;
        pos.x += (needleRight ? 1 : -1) * needleSpeed * Time.deltaTime;

        if (pos.x > needleStartX + range)
        {
            pos.x = needleStartX + range;
            needleRight = false;
        }
        else if (pos.x < needleStartX - range)
        {
            pos.x = needleStartX - range;
            needleRight = true;
        }

        needle.anchoredPosition = pos;
    }

    void MoveGreenZone()
    {
        Vector2 pos = greenZone.anchoredPosition;
        pos.x += (greenRight ? 1 : -1) * greenMoveSpeed * Time.deltaTime;

        if (pos.x > greenStartX + range)
        {
            pos.x = greenStartX + range;
            greenRight = false;
        }
        else if (pos.x < greenStartX - range)
        {
            pos.x = greenStartX - range;
            greenRight = true;
        }

        greenZone.anchoredPosition = pos;
    }

    void StopNeedle()
    {
        stopped = true;

        float needleX = needle.anchoredPosition.x;
        float greenMin = greenZone.anchoredPosition.x - greenZone.rect.width / 2f;
        float greenMax = greenZone.anchoredPosition.x + greenZone.rect.width / 2f;

        if (needleX >= greenMin && needleX <= greenMax)
        {
            Debug.Log($"Phase {phase} 成功！");
            Invoke(nameof(NextPhase), 0.4f);
        }
        else
        {
            Debug.Log("失敗！");
            Invoke(nameof(ResetNeedle), 0.4f);
        }
    }

    void ResetNeedle()
    {
        needle.anchoredPosition =
            new Vector2(needleStartX, needle.anchoredPosition.y);
        stopped = false;
    }

    void NextPhase()
    {
        phase++;
        stopped = false;

        if (phase > 3)
        {
            Debug.Log("水圧ゲージミニゲーム クリア！");

            // 今のミニゲームを消す
            gameObject.SetActive(false);

            // 次のミニゲームへ
            if (nextMiniGamePanel != null)
                nextMiniGamePanel.SetActive(true);

            return;
        }

        UpdatePhase();
    }

    void UpdatePhase()
    {
        if (phase == 1)
        {
            greenMoveSpeed = 0f;
            needleSpeed = 200f;
            Debug.Log("Phase1：緑は動かない");
        }
        else if (phase == 2)
        {
            greenMoveSpeed = 60f;
            needleSpeed = 230f;
            Debug.Log("Phase2：緑がゆっくり動く");
        }
        else if (phase == 3)
        {
            greenMoveSpeed = 350f;   
            needleSpeed = 550f;
            Debug.Log("Phase3：緑が高速");
        }
    }
    public void ResetGame()
    {
        phase = 1;
        stopped = false;

        greenRight = true;
        needleRight = true;

        // 位置を初期位置に戻す
        greenZone.anchoredPosition =
            new Vector2(greenStartX, greenZone.anchoredPosition.y);

        needle.anchoredPosition =
            new Vector2(needleStartX, needle.anchoredPosition.y);

        UpdatePhase();

        Debug.Log("PressureMiniGame Reset");
    }
}

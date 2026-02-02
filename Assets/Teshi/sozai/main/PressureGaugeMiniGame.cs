using UnityEngine;
using UnityEngine.InputSystem;

public class PressureMiniGame : MonoBehaviour
{
    [Header("UI")]
    public RectTransform greenZone;
    public RectTransform needle;

    [Header("Progress")]
    public int requiredSuccess = 6;
    int successCount = 0;

    [Header("Next Game")]
    public GameObject finalDecisionMiniGame;

    [Header("SE")]
    public AudioSource audioSource;
    public AudioClip successSE;
    public AudioClip failSE;
    public AudioClip finalSuccessSE;

    float greenMoveSpeed = 0f;
    float needleSpeed = 200f;

    float range = 200f;
    float greenStartX;
    float needleStartX;

    bool greenRight = true;
    bool needleRight = true;
    bool stopped = false;

    // ★ 追加（超重要）
    bool isGameActive = false;
    bool isGameFinished = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        greenStartX = greenZone.anchoredPosition.x;
        needleStartX = needle.anchoredPosition.x;

        ResetGame(); // ← ここでゲーム開始
    }

    void Update()
    {
        // ★ 完全ガード
        if (!isGameActive || isGameFinished) return;
        if (stopped) return;

        MoveNeedle();

        if (successCount >= 2)
            MoveGreenZone();
    }

    // ===== 針移動 =====
    void MoveNeedle()
    {
        Vector2 pos = needle.anchoredPosition;
        pos.x += (needleRight ? 1 : -1) * needleSpeed * Time.deltaTime;

        if (pos.x > needleStartX + range) { pos.x = needleStartX + range; needleRight = false; }
        if (pos.x < needleStartX - range) { pos.x = needleStartX - range; needleRight = true; }

        needle.anchoredPosition = pos;
    }

    // ===== 緑ゾーン移動 =====
    void MoveGreenZone()
    {
        Vector2 pos = greenZone.anchoredPosition;
        pos.x += (greenRight ? 1 : -1) * greenMoveSpeed * Time.deltaTime;

        if (pos.x > greenStartX + range) { pos.x = greenStartX + range; greenRight = false; }
        if (pos.x < greenStartX - range) { pos.x = greenStartX - range; greenRight = true; }

        greenZone.anchoredPosition = pos;
    }

    // ===== 入力（Input System）=====
    public void OnButtom(InputAction.CallbackContext context)
    {
        if (!isGameActive || isGameFinished) return;
        if (context.started)
            StopNeedle();
    }

    // ===== 判定 =====
    void StopNeedle()
    {
        stopped = true;

        float needlePos = needle.anchoredPosition.x;
        float greenCenter = greenZone.anchoredPosition.x;
        float halfSize = greenZone.rect.width * 0.5f;

        if (needlePos >= greenCenter - halfSize && needlePos <= greenCenter + halfSize)
        {
            OnSuccess();
        }
        else
        {
            if (audioSource && failSE)
                audioSource.PlayOneShot(failSE);

            ResetRound();
        }
    }

    // ===== 成功 =====
    void OnSuccess()
    {
        successCount++;
        Debug.Log($"SUCCESS {successCount}/{requiredSuccess}");

        if (successCount < requiredSuccess)
        {
            if (audioSource && successSE)
                audioSource.PlayOneShot(successSE);

            UpdateDifficulty();
            ResetRound();
            return;
        }

        FinishGame();
    }

    // ===== 難易度（あなたの数値そのまま）=====
    void UpdateDifficulty()
    {
        greenMoveSpeed = 0f;
        needleSpeed = 200f;

        if (successCount == 1) { greenMoveSpeed = 80f; needleSpeed = 260f; }
        if (successCount == 2) { greenMoveSpeed = 80f; needleSpeed = 260f; }
        if (successCount == 3) { greenMoveSpeed = 250f; needleSpeed = 470f; }
        if (successCount == 4) { greenMoveSpeed = 600f; needleSpeed = 20f; }
        if (successCount == 5) { greenMoveSpeed = 520f; needleSpeed = 400f; }
    }

    // ===== ラウンドリセット =====
    void ResetRound()
    {
        stopped = false;
        needleRight = true;
        greenRight = true;

        needle.anchoredPosition = new Vector2(needleStartX, needle.anchoredPosition.y);
        greenZone.anchoredPosition = new Vector2(greenStartX, greenZone.anchoredPosition.y);
    }

    // ===== 外部リセット =====
    public void ResetGame()
    {
        successCount = 0;
        stopped = false;
        isGameActive = true;
        isGameFinished = false;

        UpdateDifficulty();
        ResetRound();

        Debug.Log("PressureMiniGame Start");
    }

    // ===== 完全クリア =====
    void FinishGame()
    {
        if (isGameFinished) return;

        isGameFinished = true;
        isGameActive = false;

        Debug.Log("水圧ゲージ 完全クリア");

        if (finalSuccessSE)
            SEManager.Instance.PlaySE(finalSuccessSE);

        gameObject.SetActive(false);

        if (finalDecisionMiniGame)
            finalDecisionMiniGame.SetActive(true);
    }
    public void ForceStop()
    {
        isGameActive = false;
        isGameFinished = true;
        stopped = true;

        Debug.Log("PressureMiniGame ForceStopped");
    }
}

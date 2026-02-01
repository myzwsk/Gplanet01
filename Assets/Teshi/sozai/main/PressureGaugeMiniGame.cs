using UnityEngine;

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
    bool isVertical = false;

    void Start()
    {
        // ★ AudioSource 自動取得（入れ忘れ防止）
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        greenStartX = greenZone.anchoredPosition.x;
        needleStartX = needle.anchoredPosition.x;
        ResetRound();
        UpdateDifficulty();
    }

    void Update()
    {
        if (stopped) return;

        MoveNeedle();

        if (successCount >= 2)
            MoveGreenZone();

        if (Input.GetKeyDown(KeyCode.Space))
            StopNeedle();
    }

    // ===== 針移動 =====
    void MoveNeedle()
    {
        Vector2 pos = needle.anchoredPosition;

        if (!isVertical)
            pos.x += (needleRight ? 1 : -1) * needleSpeed * Time.deltaTime;
        else
            pos.y += (needleRight ? 1 : -1) * needleSpeed * Time.deltaTime;

        float limit = range;

        if (!isVertical)
        {
            if (pos.x > needleStartX + limit) { pos.x = needleStartX + limit; needleRight = false; }
            if (pos.x < needleStartX - limit) { pos.x = needleStartX - limit; needleRight = true; }
        }
        else
        {
            if (pos.y > limit) { pos.y = limit; needleRight = false; }
            if (pos.y < -limit) { pos.y = -limit; needleRight = true; }
        }

        needle.anchoredPosition = pos;
    }

    // ===== 緑ゾーン移動 =====
    void MoveGreenZone()
    {
        Vector2 pos = greenZone.anchoredPosition;

        if (!isVertical)
            pos.x += (greenRight ? 1 : -1) * greenMoveSpeed * Time.deltaTime;
        else
            pos.y += (greenRight ? 1 : -1) * greenMoveSpeed * Time.deltaTime;

        if (!isVertical)
        {
            if (pos.x > greenStartX + range) { pos.x = greenStartX + range; greenRight = false; }
            if (pos.x < greenStartX - range) { pos.x = greenStartX - range; greenRight = true; }
        }
        else
        {
            if (pos.y > range) { pos.y = range; greenRight = false; }
            if (pos.y < -range) { pos.y = -range; greenRight = true; }
        }

        greenZone.anchoredPosition = pos;
    }

    // ===== 判定 =====
    void StopNeedle()
    {
        stopped = true;

        float needlePos = isVertical ? needle.anchoredPosition.y : needle.anchoredPosition.x;
        float greenCenter = isVertical ? greenZone.anchoredPosition.y : greenZone.anchoredPosition.x;
        float halfSize = (isVertical ? greenZone.rect.height : greenZone.rect.width) * 0.5f;

        if (needlePos >= greenCenter - halfSize && needlePos <= greenCenter + halfSize)
        {
            OnSuccess();
        }
        else
        {
            // ★ 失敗SE
            if (audioSource != null && failSE != null)
                audioSource.PlayOneShot(failSE);

            ResetRound();
        }
    }

    // ===== 成功処理 =====
    void OnSuccess()
    {
        // ★ 通常成功SE（最終以外）
        if (audioSource != null && successSE != null && successCount + 1 < requiredSuccess)
        {
            audioSource.PlayOneShot(successSE);
        }

        successCount++;
        Debug.Log($"SUCCESS {successCount}/{requiredSuccess}");

        if (successCount >= requiredSuccess)
        {
            FinishGame(); // ← 最終SEはここで鳴らす
            return;
        }

        UpdateDifficulty();
        ResetRound();
    }

    // ===== 難易度（あなたの数値そのまま）=====
    void UpdateDifficulty()
    {
        greenMoveSpeed = 0f;
        needleSpeed = 200f;

        if (successCount >= 1 && successCount < 2)
        {
            greenMoveSpeed = 80f;
            needleSpeed = 260f;
        }
        else if (successCount >= 2 && successCount < 3)
        {
            greenMoveSpeed = 80f;
            needleSpeed = 260f;
        }
        else if (successCount >= 3 && successCount < 4)
        {
            greenMoveSpeed = 250f;
            needleSpeed = 470f;
        }
        else if (successCount >= 4 && successCount < 5)
        {
            greenMoveSpeed = 600f;
            needleSpeed = 20f;
        }
        else if (successCount >= 5 && successCount < 6)
        {
            greenMoveSpeed = 520f;
            needleSpeed = 400f;
        }
    }

    // ===== ラウンドリセット =====
    void ResetRound()
    {
        stopped = false;
        needleRight = true;
        greenRight = true;

        if (!isVertical)
        {
            needle.anchoredPosition = new Vector2(needleStartX, needle.anchoredPosition.y);
            greenZone.anchoredPosition = new Vector2(greenStartX, greenZone.anchoredPosition.y);
        }
        else
        {
            needle.anchoredPosition = Vector2.zero;
            greenZone.anchoredPosition = Vector2.zero;
        }
    }

    public void ResetGame()
    {
        successCount = 0;
        isVertical = false;

        GetComponent<RectTransform>().localRotation = Quaternion.identity;

        UpdateDifficulty();
        ResetRound();

        Debug.Log("PressureMiniGame Reset");
    }

    // ===== クリア =====
    void FinishGame()
    {
        Debug.Log("水圧ゲージ 完全クリア");

        // ★ 最終クリアSE（ここが本命）
        if (finalSuccessSE != null)
            SEManager.Instance.PlaySE(finalSuccessSE);

        gameObject.SetActive(false);

        if (finalDecisionMiniGame != null)
            finalDecisionMiniGame.SetActive(true);
    }
}

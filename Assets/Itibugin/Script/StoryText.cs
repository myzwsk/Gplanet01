using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI textUI;
    [Header("Story")]
    [TextArea(2, 5)]
    public string[] lines;
    public Sprite[] backgrounds;
    [Header("Move")]
    public float speed = 300f;
    public Vector2 startPos = new Vector2(-800, 0);
    public float stopX = 0f;
    [Header("Fade Settings")]
    public int[] fadeLines; // フェードさせたい行番号（0始まり）

    [Header("Background Change (Last Line)")]
    public BackGround backGround;
    public Sprite lastBackground;
    [Header("Background Change Timing")]
    public int changeAtLine = 3; // ← ここで指定（0始まり）
    [Header("End Scene")]
    public Screenfade screenfade;

    int index = 0;
    bool isMoving = false;
    bool finished = false;

    void Start()
    {
        ShowNextLine();
    }

    void Update()
    {
        // テキスト移動
        if (isMoving)
        {
            textUI.rectTransform.anchoredPosition +=
                Vector2.right * speed * Time.deltaTime;

            if (textUI.rectTransform.anchoredPosition.x >= stopX)
            {
                isMoving = false;
            }
        }

        // クリックで次へ
        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving || finished) return;
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (index >= lines.Length)
        {
            finished = true;

            // ★ 最後までいったら画面フェードアウト
            if (screenfade != null)
            {
                screenfade.FadeOutAndLoad();
            }

            return;
        }

        textUI.text = lines[index];
        textUI.rectTransform.anchoredPosition = startPos;

        if (backGround != null)
        {
            // ★ 指定Lineで lastBackground に変更
            if (index == changeAtLine && lastBackground != null)
            {
                backGround.ChangeBackground(lastBackground);
            }
            // ★ 通常背景
            else if (backgrounds != null &&
                     index < backgrounds.Length &&
                     backgrounds[index] != null)
            {
                if (ShouldFade(index))
                    backGround.ChangeBackground(backgrounds[index]);
                else
                    backGround.ChangeBackgroundImmediate(backgrounds[index]);
            }
        }

        index++;
        isMoving = true;
    }
    bool ShouldFade(int lineIndex)
    {
        if (fadeLines == null) return false;

        foreach (int i in fadeLines)
        {
            if (i == lineIndex) return true;
        }
        return false;
    }
}

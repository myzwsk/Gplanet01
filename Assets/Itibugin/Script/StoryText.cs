using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    bool inputLocked = false;
    bool waitingForFade = false;
    void Start()
    {
        ShowNextLine();
    }
    
    public void OnAction(InputAction.CallbackContext context)//PadのB
    {
        if (context.performed)// <-performed（ボタンが深く押された瞬間
        {
            NextAction();
        }
    }
    void Update()
    {
        if (inputLocked) return;

        // テキスト移動
        if (isMoving)
        {
            textUI.rectTransform.anchoredPosition +=
                Vector2.right * speed * Time.deltaTime;

            if (textUI.rectTransform.anchoredPosition.x >= stopX)
                isMoving = false;
        }

        // クリック
        if (Input.GetMouseButtonDown(0))
        {
            NextAction();
        }
    }

    void ShowNextLine()
    {
        // ★ すでにフェード開始してたら何もしない
        if (waitingForFade) return;

        // ★ これから表示する行が「最後」
        bool isLastLine = (index == lines.Length - 1);
        textUI.text = lines[index];
        textUI.rectTransform.anchoredPosition = startPos;

        // 背景
        if (backGround != null)
        {
            if (index == changeAtLine && lastBackground != null)
            {
                backGround.ChangeBackground(lastBackground);
            }
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
    void NextAction()
    {

        if (isMoving) return;

        // ★ すでに最後を表示し終わっていたらフェード
        if (index >= lines.Length)
        {
            inputLocked = true;

            if (screenfade != null)
                screenfade.FadeOutAndLoad();

            return;
        }

        ShowNextLine();
    }
}

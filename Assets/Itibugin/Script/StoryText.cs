using TMPro;
using UnityEngine;

public class StoryText : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    [TextArea(2, 4)]
    public string[] lines;

    public float speed = 600f;

    Vector2 startPos = new Vector2(1200, -300);
    Vector2 targetPos = new Vector2(0, -300);

    int index = 0;
    bool isMoving = false;

    void Start()
    {
        ShowNextLine();
    }

    void Update()
    {
        // テキスト移動
        if (isMoving)
        {
            textUI.rectTransform.anchoredPosition =
                Vector2.MoveTowards(
                    textUI.rectTransform.anchoredPosition,
                    targetPos,
                    speed * Time.deltaTime
                );

            if (Vector2.Distance(
                textUI.rectTransform.anchoredPosition,
                targetPos) < 1f)
            {
                isMoving = false;
            }
        }

        // クリックで次へ
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (index >= lines.Length) return;

        textUI.text = lines[index];
        textUI.rectTransform.anchoredPosition = startPos;

        index++;
        isMoving = true;
    }
}

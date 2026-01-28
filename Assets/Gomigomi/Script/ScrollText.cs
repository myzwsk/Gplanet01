using UnityEngine;
using TMPro;
public class ScrollText : MonoBehaviour
{
    public float scrollSpeed = -20f;
    public float startPosition = 500f;  // テキストが動き出す位置
    public float resetPosition = -500f; // テキストが消えきる位置

    private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 上方向に移動
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        // 一定位置まで行ったらループ（必要に応じて）
        if (rectTransform.anchoredPosition.y > resetPosition)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startPosition);
        }

    }
}

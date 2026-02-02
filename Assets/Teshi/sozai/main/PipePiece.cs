using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PipePiece : MonoBehaviour, IPointerClickHandler
{
    [Header("Rotation")]
    public int correctRotation;   // 0 / 90 / 180 / 270

    RectTransform rect;
    Image img;

    int currentRotation;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        if (rect == null)
        {
            Debug.LogError($"{name} : RectTransform がありません");
            enabled = false;
            return;
        }

        if (img == null)
        {
            Debug.LogError($"{name} : Image がありません");
            enabled = false;
            return;
        }

        RandomizeRotation();
    }

    // ★ マウスクリック
    public void OnPointerClick(PointerEventData eventData)
    {
        Rotate();
    }

    // ★ コントローラー用（PipeGameManager から呼ぶ）
    public void Rotate()
    {
        currentRotation = (currentRotation + 90) % 360;
        if (rect != null) rect.localEulerAngles = new Vector3(0, 0, currentRotation);
    }

    // ★ 初期化・リセット用
    public void RandomizeRotation()
    {
        int[] angles = { 0, 90, 180, 270 };
        currentRotation = angles[Random.Range(0, angles.Length)];
        if (rect != null) rect.localEulerAngles = new Vector3(0, 0, currentRotation);
    }

    public bool IsCorrect()
    {
        return currentRotation == correctRotation;
    }

    void Update()
    {
        // 正解した配管を光らせる
        img.color = IsCorrect() ? Color.yellow : Color.white;
    }
}

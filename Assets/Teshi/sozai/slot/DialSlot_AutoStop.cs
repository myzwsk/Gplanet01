using UnityEngine;
using UnityEngine.InputSystem;

public class DialSlot_AutoStop : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform dial;   // 回る円盤

    [Header("Slots (模様の数)")]
    [SerializeField] private int slotCount = 12;         // 12分割（30度刻み）
    [SerializeField] private int winSlotIndex = 0;       // 当たりスロット番号

    [Header("Spin")]
    [SerializeField] private float spinSpeed = 540f;     // 自動回転速度(度/秒)
    [SerializeField] private float decelSpeed = 900f;    // 減速速度
    [SerializeField] private float snapSpeed = 20f;      // スナップ吸い付き

    [Header("Judge (ビタ判定)")]
    [SerializeField] private float perfectEpsilon = 1.0f; // PERFECT(ビタ)許容角度
    [SerializeField] private float goodEpsilon = 6.0f;    // GOOD許容角度

    private enum State { AutoSpin, Decelerating, Snapping }
    private State state = State.AutoSpin;

    private float currentSpeed;
    private float targetAngle;

    private float SlotStep => 360f / slotCount;

    void Start()
    {
        currentSpeed = spinSpeed;

        if (dial == null)
        {
            Debug.LogError("dial が未設定です。Inspectorで DialMesh を入れてください。");
        }
    }

    void Update()
    {
        if (dial == null) return;

        // コントローラーが無ければ動かさない（PCなら接続してね）
        if (Gamepad.current == null) return;

        // A（PSなら×）で止める
        bool stopPressed = false;

        // コントローラー（A / ×）
        if (Gamepad.current != null)
        {
            stopPressed |= Gamepad.current.buttonSouth.wasPressedThisFrame;
        }

        // キーボード（Space）
        if (Keyboard.current != null)
        {
            stopPressed |= Keyboard.current.spaceKey.wasPressedThisFrame;
        }


        // 入力は AutoSpin の時だけ受け付ける
        if (stopPressed && state == State.AutoSpin)
        {
            state = State.Decelerating;
        }

        switch (state)
        {
            case State.AutoSpin:
                dial.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
                break;

            case State.Decelerating:
                // 速度を0へ落とす
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, decelSpeed * Time.deltaTime);
                dial.Rotate(0f, currentSpeed * Time.deltaTime, 0f, Space.Self);

                // ほぼ止まったらスナップへ
                if (currentSpeed <= 5f)
                {
                    targetAngle = GetNearestSlotAngle();
                    state = State.Snapping;
                }
                break;

            case State.Snapping:
                // 最寄りのスロット角へ吸い付く
                float y = dial.localEulerAngles.y;
                float next = Mathf.LerpAngle(y, targetAngle, 1f - Mathf.Exp(-snapSpeed * Time.deltaTime));
                dial.localEulerAngles = new Vector3(0f, next, 0f);

                // ほぼ一致したら確定
                if (Mathf.Abs(Mathf.DeltaAngle(next, targetAngle)) < 0.3f)
                {
                    dial.localEulerAngles = new Vector3(0f, targetAngle, 0f);

                    Judge();

                    // 次のラウンド
                    currentSpeed = spinSpeed;
                    state = State.AutoSpin;
                }
                break;
        }
    }

    // 今の角度を「スロット刻み」に丸めて、止まる角度を作る
    private float GetNearestSlotAngle()
    {
        float y = NormalizeAngle(dial.localEulerAngles.y);
        return Mathf.Round(y / SlotStep) * SlotStep;
    }

    // ビタ判定（PERFECT/GOOD/MISS）
    private void Judge()
    {
        float y = NormalizeAngle(dial.localEulerAngles.y);
        float winAngle = NormalizeAngle(winSlotIndex * SlotStep);
        float diff = Mathf.Abs(Mathf.DeltaAngle(y, winAngle));

        if (diff <= perfectEpsilon)
            Debug.Log($"PERFECT（ビタ） diff={diff:F2}°");
        else if (diff <= goodEpsilon)
            Debug.Log($"GOOD diff={diff:F2}°");
        else
            Debug.Log($"MISS diff={diff:F2}°");
    }

    private float NormalizeAngle(float a)
    {
        a %= 360f;
        if (a < 0f) a += 360f;
        return a;
    }
}

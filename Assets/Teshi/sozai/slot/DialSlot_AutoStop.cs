using UnityEngine;
using UnityEngine.InputSystem;

public class DialSlot_AutoStop : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform dial;   // DialMesh（Tick_0～Tick_11が子にある想定）

    [Header("Slots")]
    [SerializeField] private int slotCount = 12;
    [SerializeField] private int winSlotIndex = 0;

    [Header("Spin (GTAダイヤル＝Y軸)")]
    [SerializeField] private float spinSpeed = 540f;   // 度/秒

    [Header("Judge (角度で判定)")]
    [SerializeField] private float perfectEpsilon = 1.0f; // PERFECT許容角度
    [SerializeField] private float goodEpsilon = 6.0f;    // GOOD許容角度

    [Header("Control")]
    [SerializeField] private bool restartOnPressWhenStopped = true;

    private enum State { AutoSpin, Stopped }
    private State state = State.AutoSpin;

    private float SlotStep => 360f / slotCount;

    void Start()
    {
        if (dial == null)
        {
            Debug.LogError("dial が未設定です。Inspectorで DialMesh を入れてください。");
            enabled = false;
            return;
        }

        // 最初から当たりだけ赤
        UpdateTickColors(-1);
    }

    void Update()
    {
        if (dial == null) return;

        // 入力（A/× or Space）
        bool press = false;
        if (Gamepad.current != null) press |= Gamepad.current.buttonSouth.wasPressedThisFrame;
        if (Keyboard.current != null) press |= Keyboard.current.spaceKey.wasPressedThisFrame;

        // 止まってる時に押したら再開（任意）
        if (press && state == State.Stopped && restartOnPressWhenStopped)
        {
            state = State.AutoSpin;
            UpdateTickColors(-1); // 黄色を消して当たりだけ赤に戻す
            return;
        }

        switch (state)
        {
            case State.AutoSpin:
                // 回転
                dial.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);

                // ★ビタ押し：押した瞬間に即停止
                if (press)
                {
                    JudgeAndColorizeY(); // 押した瞬間の角度で判定
                    state = State.Stopped;
                }
                break;

            case State.Stopped:
                // 完全停止（何もしない）
                break;
        }
    }

    private void JudgeAndColorizeY()
    {
        float y = NormalizeAngle(dial.localEulerAngles.y);
        float winAngle = NormalizeAngle(winSlotIndex * SlotStep);
        float diff = Mathf.Abs(Mathf.DeltaAngle(y, winAngle));

        // 押した瞬間の角度が「どのスロットに一番近いか」
        int stoppedIndex = Mathf.RoundToInt(y / SlotStep) % slotCount;

        // 色：当たり赤、押した場所黄
        UpdateTickColors(stoppedIndex);

        if (diff <= perfectEpsilon)
            Debug.Log($"PERFECT（ビタ） Tick_{stoppedIndex} diff={diff:F2}°");
        else if (diff <= goodEpsilon)
            Debug.Log($"GOOD Tick_{stoppedIndex} diff={diff:F2}°");
        else
            Debug.Log($"MISS Tick_{stoppedIndex} diff={diff:F2}°");
    }

    private void UpdateTickColors(int stoppedIndex)
    {
        for (int i = 0; i < slotCount; i++)
        {
            Transform t = dial.Find($"Tick_{i}");
            if (t == null) continue;

            Renderer r = t.GetComponent<Renderer>();
            if (r == null) continue;

            if (i == winSlotIndex) r.material.color = Color.red;           // 当たり
            else if (i == stoppedIndex) r.material.color = Color.yellow;   // 押した位置
            else r.material.color = Color.white;                           // それ以外
        }
    }

    private float NormalizeAngle(float a)
    {
        a %= 360f;
        if (a < 0f) a += 360f;
        return a;
    }
}

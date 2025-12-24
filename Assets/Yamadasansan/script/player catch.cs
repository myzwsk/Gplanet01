using UnityEngine;

public class playercatch : MonoBehaviour
{
    [HideInInspector] public Transform playerTransform;
    public float fromCenter = 1.2f;
    public bool isGrabIdol=false;
    public bool isPushing { get; private set; }
    public bool isPulling { get; private set; }

    private Vector3 previousPosition;
    private const float MovementThreshold = 0.00000001f;

    // 🔴 判定維持用のタイマー
    private float stopDelayTimer = 0f;
    private const float StopDelayDuration = 0.15f; // 0.15秒間は「止まった」とみなさない

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            ResetStates();
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 moveDelta = currentPosition - previousPosition;
        float speed = moveDelta.magnitude;

        if (speed > MovementThreshold)
        {
            // 動きを検知したらタイマーをリセットして判定
            stopDelayTimer = StopDelayDuration;

            float dot = Vector3.Dot(playerTransform.forward, moveDelta.normalized);

            // 押し引きの状態を更新
            isGrabIdol = true;
            isPushing = (dot > 0.1f);
            isPulling = (dot < -0.1f);
        }
        else
        {
            // 動きが止まったらタイマーを減らす
            if (stopDelayTimer > 0)
            {
                stopDelayTimer -= Time.deltaTime;
            }
            else
            {
                // タイマーが切れて初めて、フラグをfalseにする
                isGrabIdol = false;
                isPushing = false;
                isPulling = false;
            }
        }

        previousPosition = currentPosition;
    }

    public void ResetStates()
    {
        isGrabIdol = false; 
        isPushing = false;
        isPulling = false;
        stopDelayTimer = 0;
        previousPosition = transform.position;
    }
}
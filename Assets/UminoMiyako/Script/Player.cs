using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float initialJumpVelocity = 2f; // 最初のジャンプの勢い
    public float gravityMultiplier = 1f; // ジャンプ上昇中の重力抑制用
    public float movelock = 0.5f;
    public float maxSpeed = 5f;          // 最大速度
    public float acceleration = 25f;     // 加速の強さ（力をシミュレート）
    public float deceleration = 30f;     // 減速の強さ（キーを離した時の摩擦をシミュレート）

    private PlayerHand hand;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isJumping = false;
    private float maxJumpVelocity = 0f; // ボタンを離した時点での最高速度を制限する値

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hand = GetComponent<PlayerHand>();
    }

    // Move アクションに紐付ける
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Jump アクションに紐付ける
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 地面にいる場合のみジャンプを開始
            if (controller.isGrounded)
            {
                if (!hand.isGrabbing)   //ものを持っている間はジャンプできない
                {
                    // 初期ジャンプ速度を設定
                    velocity.y = initialJumpVelocity;
                    isJumping = true; // ジャンプ中フラグを立てる
                    maxJumpVelocity = initialJumpVelocity; // 初期値として最大ジャンプ速度を設定
                }
                
            }
        }
        else if (context.canceled)
        {
            // ボタンを離した瞬間

            // 現在のY軸の速度と、初期速度のうち小さい方を最大速度として設定
            // これにより、短く押すほどvelocity.yが早く0に近づき、ジャンプが短くなります。
            maxJumpVelocity = Mathf.Min(velocity.y, initialJumpVelocity);
            isJumping = false; // ジャンプ中フラグを下ろす
        }
    }
    void Update()
    {
        // カメラの参照を取得（メインカメラを使用）
        Transform cam = Camera.main.transform;

        // カメラの forward と right を水平成分だけにする
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0;
        camRight.Normalize();

        // 入力をカメラ基準に変換
        Vector3 targetDirection = camForward * moveInput.y + camRight * moveInput.x;

        // 現在の水平方向の速度ベクトル（Y成分を無視）
        Vector3 currentHorizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

        if (targetDirection.magnitude > 0.1f) // キーが押されている場合（加速）
        {
            if (controller.isGrounded)
            {
                currentHorizontalVelocity += targetDirection.normalized * acceleration * Time.deltaTime;
            }
            else
            {
                currentHorizontalVelocity += targetDirection.normalized * acceleration * Time.deltaTime * movelock;
            }

            if (controller.isGrounded)
            {
                currentHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontalVelocity, maxSpeed - (1f * hand.catchObjectFlag));
            }
            else
            {
                currentHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontalVelocity, maxSpeed * 0.6f);
            }

            // キャラクターの向きを移動方向へ回転させる
            if (targetDirection != Vector3.zero && !hand.isGrabbing)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
            }
        }
        else
        {
            if (controller.isGrounded)
            {
                currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero, deceleration * Time.deltaTime);
            }
        }

        // --- 以下はジャンプと重力処理はそのまま ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isJumping = false;
        }

        if (isJumping && velocity.y > 0)
        {
            velocity.y += gravity * Time.deltaTime * (1f / gravityMultiplier);
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (!isJumping && velocity.y > maxJumpVelocity)
        {
            velocity.y = maxJumpVelocity;
        }

        velocity.x = currentHorizontalVelocity.x;
        velocity.z = currentHorizontalVelocity.z;

        controller.Move(velocity * Time.deltaTime);
    }

    //宮澤彩姫バブルscore
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bubble"))
        {
            Destroy(other.gameObject);
        }
    }
}
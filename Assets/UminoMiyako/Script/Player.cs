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

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity
        ;
    private bool isJumping = false;
    private float maxJumpVelocity = 0f; // ボタンを離した時点での最高速度を制限する値

    void Start()
    {
        controller = GetComponent<CharacterController>();
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
                // 初期ジャンプ速度を設定
                velocity.y = initialJumpVelocity;
                isJumping = true; // ジャンプ中フラグを立てる
                maxJumpVelocity = initialJumpVelocity; // 初期値として最大ジャンプ速度を設定
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
        // 1. 水平方向の速度を計算（力をシミュレート）

        // 目標の移動方向ベクトル
        Vector3 targetDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // 現在の水平方向の速度ベクトル（Y成分を無視）
        Vector3 currentHorizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

        if (targetDirection.magnitude > 0.1f) // キーが押されている場合（加速）
        {
            // 加速（力を加える）
            // 加速力 * 時間 = 速度の変化
            if (controller.isGrounded)
            {
                currentHorizontalVelocity += targetDirection.normalized * acceleration * Time.deltaTime;
            }
            else
            {
                currentHorizontalVelocity += targetDirection.normalized * acceleration * Time.deltaTime * movelock;
            }
            // 最大速度で制限
            if(controller.isGrounded)
            {
                currentHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontalVelocity, maxSpeed);
            }
            else
            {
                currentHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontalVelocity, maxSpeed*0.6f);
            }

            // キャラクターの向きを移動方向へ回転させる（お好みで）
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
            }
        }
        else // キーが離されている場合（減速/摩擦）
        {
            // 減速（摩擦力をシミュレート）
            if (controller.isGrounded)
            {
                currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero, deceleration * Time.deltaTime);
            }
            
        }

        // 2. 重力処理とジャンプの上昇制御

        // 地面にいるかどうかの判定と速度リセット
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 地面スレスレに保つための微小な負の値
            isJumping = false; // 地面についたらジャンプ中ではない
        }

        // ジャンプの上昇制御
        if (isJumping && velocity.y > 0)
        {
            // ボタンを離しておらず、かつ上昇中 (velocity.y > 0) の場合、重力を弱くする (より長く上昇)
            velocity.y += gravity * Time.deltaTime * (1f / gravityMultiplier);
        }
        else
        {
            // ボタンを離した、または下降中の場合、通常の重力を適用
            velocity.y += gravity * Time.deltaTime;
        }

        // ボタンを離した際の最高速度の制限（短押しジャンプの実現）
        // maxJumpVelocityよりもvelocity.yが大きい場合、maxJumpVelocityに制限する
        // ただし、下降中は制限しない
        if (!isJumping && velocity.y > maxJumpVelocity)
        {
            velocity.y = maxJumpVelocity;
        }
        // 3. 最終的な速度を統合し、CharacterController.Move()で適用

        // velocity変数に水平方向の新しい速度を再代入
        velocity.x = currentHorizontalVelocity.x;
        velocity.z = currentHorizontalVelocity.z;
        // 3. 垂直方向の移動を適用
        controller.Move(velocity * Time.deltaTime);
    }
}
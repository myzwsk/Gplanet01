using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float gravity = -9.81f; // 重力計算用
    public float initialJumpVelocity = 2f; // 最初のジャンプの勢い
    public float gravityMultiplier = 1f; // ジャンプ上昇中の重力抑制用
    public float movelock = 0.5f; // 空中時の移動速度制限用
    public float maxSpeed = 5f; // 最大速度
    public float acceleration = 25f; // 加速の強さ
    public float deceleration = 30f; // 減速の強さ
    public float climbSpeed = 3f; // 梯子用の速度

    private PlayerHand hand; // つかみ時オブジェクトの吸着位置
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isJumping = false;
    private bool isClimbing = false; // 梯子判定フラグ
    private float maxJumpVelocity = 0f; // ボタンを離した時点での最高速

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hand = GetComponent<PlayerHand>();
    }

    public void OnMove(InputAction.CallbackContext context) // Moveアクション
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) // Jumpアクション
    {
        if (isClimbing) return; // 梯子中はジャンプ禁止

        if (context.started) // ジャンプボタン押し始め
        {
            if (controller.isGrounded) // 地面にいる間ジャンプ可
            {
                if (!hand.isGrabbing) // ものをキャッチしていないときジャンプ可
                {
                    velocity.y = initialJumpVelocity;
                    isJumping = true;
                    maxJumpVelocity = initialJumpVelocity;
                }
            }
        }
        else if (context.canceled) // ジャンプボタンを離したとき
        {
            maxJumpVelocity = Mathf.Min(velocity.y, initialJumpVelocity);
            isJumping = false;
        }
    }

    void Update()
    {
        // カメラの座標取得
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
        Vector3 currentHorizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

        if (!isClimbing) // 通常移動
        {
            if (targetDirection.magnitude > 0.1f) // キーが押されている場合（加速）
            {
                // 加速
                if (controller.isGrounded)
                    currentHorizontalVelocity += targetDirection.normalized * acceleration * Time.deltaTime;
                else
                    currentHorizontalVelocity += targetDirection.normalized * acceleration * Time.deltaTime * movelock;
                // 速度制限
                if (controller.isGrounded)
                    currentHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontalVelocity, maxSpeed - (1f * hand.catchObjectFlag));
                else
                    currentHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontalVelocity, maxSpeed * 0.6f);
                // つかんでないときは移動方向に向きを変える
                if (targetDirection != Vector3.zero && !hand.isGrabbing)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
                }
            }
            else
            {
                if (controller.isGrounded)
                    currentHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            // 重力とジャンプ処理
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
                isJumping = false;
            }

            if (isJumping && velocity.y > 0)
                velocity.y += gravity * Time.deltaTime * (1f / gravityMultiplier);
            else
                velocity.y += gravity * Time.deltaTime;

            if (!isJumping && velocity.y > maxJumpVelocity)
                velocity.y = maxJumpVelocity;
        }
        else // 梯子移動
        {
            // 横移動は制限して、上下だけ
            currentHorizontalVelocity = Vector3.zero;

            // 上下入力で移動
            velocity.y = moveInput.y * climbSpeed;
        }

        velocity.x = currentHorizontalVelocity.x;
        velocity.z = currentHorizontalVelocity.z;

        controller.Move(velocity * Time.deltaTime);
    }
    public void ExitSlopeTop(float groundY)
    {
        isClimbing = false;
        // 足元を床の高さに合わせる
        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;
        velocity.y = -2f; // 着地扱い
    }

    public void ExitSlopeBottom(float groundY)
    {
        isClimbing = false;
        // 足元を地面の高さに合わせる
        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;
        velocity.y = -2f;
    }
    public void Slope()
    {
        isClimbing = true;
        velocity = Vector3.zero; // 入った瞬間にリセット
    }
    public void ExitSlope()
    {
        isClimbing = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bubble")) // バブル取得時
        {
            Destroy(other.gameObject);
        }

        
    }

    
}

using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercopy : MonoBehaviour
{
    public float gravity = -9.81f; // 重力計算用
    public float initialJumpVelocity = 5f; // 最初のジャンプの勢い
    public float gravityMultiplier = 0.7f; // ジャンプ上昇中の重力抑制用
    public float movelock = 0.5f; // 空中時の移動速度制限用
    public float maxSpeed = 5f; // 最大速度
    public float acceleration = 25f; // 加速の強さ
    public float deceleration = 30f; // 減速の強さ
    public float climbSpeed = 3f; // 梯子用の速度
    public bool isClimbing = false; // 梯子判定フラグ
   

    private playercatch currentTargetDetector;
    private playerhandcopy hand; // つかみ時オブジェクトの吸着位置
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isJumping = false;
    private float maxJumpVelocity = 0f; // ボタンを離した時点での最高速
    private Animator animator;//アニメーター

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hand = GetComponent<playerhandcopy>();
        animator = GetComponent<Animator>();
       
       
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
                    maxJumpVelocity = velocity.y;
                    if (animator != null)
                    {
                        animator.SetTrigger("Jump");
                    }
                }
            }
        }
        else if (context.canceled) // ジャンプボタンを離したとき
        {
            maxJumpVelocity = velocity.y;
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

        UpdateAnimation();
        if (animator != null)
        {
            // Speedパラメーターの更新
            float currentSpeed = currentHorizontalVelocity.magnitude;
            animator.SetFloat("Speed", currentSpeed);
            //Debug.Log("Current Speed: " + currentSpeed);
            // IsGroundedパラメーターの更新
            animator.SetBool("IsGrounded", controller.isGrounded);
        }

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

            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
                isJumping = false;
            }

            // ジャンプ中は重力を弱める
            if (isJumping && velocity.y > 0)
            {
                // 押している間は重力を弱める → 高く飛べる
                velocity.y += gravity * Time.deltaTime * gravityMultiplier;
            }
            else
            {
                // 通常の重力
                velocity.y += gravity * Time.deltaTime;
            }

            // ボタンを離した後は最高速度を制限
            if (!isJumping && velocity.y > maxJumpVelocity)
            {
                velocity.y = maxJumpVelocity;
            }

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
        controller.enabled = false;
        Vector3 pos = transform.position;
        pos.y = groundY;
        transform.position = pos;
        controller.enabled = true;

        // 一時的に重力を無効化して前に押し出す
        StartCoroutine(DisableGravityAndPush());
    }

    private IEnumerator DisableGravityAndPush()
    {
        // 前方向へ少し押し出す
        controller.Move(transform.forward * 0.5f);

        // 0.2秒だけ重力無効化
        yield return new WaitForSeconds(0.2f);
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
        AudioSource hitAudio = other.GetComponent<AudioSource>();
        if (other.CompareTag("Bubble")) // バブル取得時
        {

            hitAudio.Play();
            // バブルの見た目を消す (MeshRendererとColliderを無効化)
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<Collider>().enabled = false;

            // 4. 無音部分を含めたクリップの全体の長さだけ待ってから、オブジェクト破棄
            Destroy(other.gameObject, hitAudio.clip.length);
        }

        playercatch detector = other.GetComponent<playercatch>();
        if (detector != null)
        {
            // 接触したオブジェクトのDetectorを一時的に保持
            currentTargetDetector = detector;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        // 離れたオブジェクトが現在掴んでいたオブジェクトと同じか確認し、参照を解除
        if (currentTargetDetector != null && other.GetComponent<playercatch>() == currentTargetDetector)
        {
            currentTargetDetector = null;
   
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 1. 衝突相手からエレベーターのスクリプトを取得
        elevator elevatorScript = hit.gameObject.GetComponent<elevator>();

        // 衝突相手がエレベータータグを持っているか確認 (タグ設定が必須)
        if (hit.gameObject.CompareTag("Elevator"))
        {
            // 衝突面が上向き（床に乗った）かを確認
            if (hit.normal.y > 0.8f)
            {
                // ガクガク防止：プレイヤーをエレベーターの子要素にする
                if (transform.parent != hit.transform)
                {
                    transform.SetParent(hit.transform);
                }

                // 動作開始：エレベーターの公開関数を呼び出す
                if (elevatorScript != null) // スクリプトが付いているか最終確認
                {
                    elevatorScript.StartElevator(); // ★これがないと動きません！
                }
            }
            else // 側面衝突などの場合
            {
                // 側面衝突でエレベーターの子要素になっていたら解除
                if (transform.parent == hit.transform)
                {
                    transform.SetParent(null);
                }
            }
        }
    }


    void UpdateAnimation()
    {
        bool isGrabbing = hand.isGrabbing;
        playercatch detector = hand.currentDetector;

        // 現在の水平方向の速度を取得（currentHorizontalVelocity は Update 内で計算されている変数）
        // もし変数がない場合は、new Vector3(velocity.x, 0, velocity.z).magnitude を使用
        float moveSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        if (isGrabbing && detector != null)
        {
            // 🔴 掴み中の静止判定（速度が 0.1 以下の場合は「掴みアイドル」とする）
            if (moveSpeed < 0.1f)
            {
                animator.SetBool("IsGrabbingIdle", true);
                animator.SetBool("IsPushing", false);
                animator.SetBool("IsPulling", false);
            }
            else
            {
                animator.SetBool("IsGrabbingIdle", false);

                // 移動している場合は、既存の押し引き判定を実行
                if (detector.isPushing)
                {
                    animator.SetBool("IsPushing", true);
                    animator.SetBool("IsPulling", false);
                }
                else if (detector.isPulling)
                {
                    animator.SetBool("IsPushing", false);
                    animator.SetBool("IsPulling", true);
                }
                else
                {
                    // 移動はしているが押し引きが未確定な場合（Stationary）
                    animator.SetBool("IsPushing", false);
                    animator.SetBool("IsPulling", false);
                }
            }
        }
        else
        {
            // 掴んでいない時はすべてオフ
            animator.SetBool("IsGrabbingIdle", false);
            animator.SetBool("IsPushing", false);
            animator.SetBool("IsPulling", false);
        }
    }
}


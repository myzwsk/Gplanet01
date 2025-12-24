using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercopy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float gravity = -9.81f;
    public float initialJumpVelocity = 5f;
    public float gravityMultiplier = 0.7f;
    public float movelock = 0.5f;
    public float maxSpeed = 5f;
    public float acceleration = 25f;
    public float deceleration = 30f;

    [Header("Climb Settings")]
    public float climbSpeed = 3f;
    public bool isClimbing = false;

    // 内部コンポーネント
    private CharacterController controller;
    private playerhandcopy hand;
    private Animator animator;

    // 状態変数
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isJumping = false;
    private float maxJumpVelocity = 0f;
    private playercatch currentTargetDetector;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hand = GetComponent<playerhandcopy>();
        animator = GetComponent<Animator>();
    }

    // --- Input System Events ---
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isClimbing) return;

        if (context.started && controller.isGrounded && !hand.isGrabbing)
        {
            velocity.y = initialJumpVelocity;
            isJumping = true;
            maxJumpVelocity = velocity.y;
            if (animator != null) animator.SetTrigger("Jump");
        }
        else if (context.canceled)
        {
            maxJumpVelocity = velocity.y;
            isJumping = false;
        }
    }

    void Update()
    {
        ApplyMovement();
        UpdateAnimation(); // 🔴 ここでアニメーション更新を呼ぶ
    }

    private void ApplyMovement()
    {
        Transform cam = Camera.main.transform;
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;
        Vector3 targetDirection = camForward * moveInput.y + camRight * moveInput.x;

        Vector3 currentHorizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

        if (!isClimbing)
        {
            if (targetDirection.magnitude > 0.1f)
            {
                float penalty = 1f * hand.catchObjectFlag;
                float currentMax = controller.isGrounded ? (maxSpeed - penalty) : (maxSpeed * 0.6f);
                float accelRate = controller.isGrounded ? acceleration : acceleration * movelock;

                currentHorizontalVelocity += targetDirection.normalized * accelRate * Time.deltaTime;
                currentHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontalVelocity, currentMax);

                if (!hand.isGrabbing)
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

            // 重力
            if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
            velocity.y += gravity * (isJumping && velocity.y > 0 ? gravityMultiplier : 1f) * Time.deltaTime;

            if (!isJumping && velocity.y > maxJumpVelocity) velocity.y = maxJumpVelocity;
        }
        else
        {
            currentHorizontalVelocity = Vector3.zero;
            velocity.y = moveInput.y * climbSpeed;
        }

        velocity.x = currentHorizontalVelocity.x;
        velocity.z = currentHorizontalVelocity.z;
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        // 通常の移動速度
        float hSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;
        animator.SetFloat("Speed", hSpeed);
        animator.SetBool("IsGrounded", controller.isGrounded);

        // 掴み中の処理
        if (hand.isGrabbing && hand.currentDetector != null)
        {
            // 物理的に動いているか、または移動キーを入力しているか
            bool isMoving = hSpeed > 0.1f || moveInput.magnitude > 0.1f;

            if (isMoving)
            {
                
                // 移動中：Detectorの判定をそのまま流し込む
                animator.SetBool("IsPushing", hand.currentDetector.isPushing);
                animator.SetBool("IsPulling", hand.currentDetector.isPulling);
                animator.SetBool("IsGrabbingIdle", hand.currentDetector.isGrabIdol);
            }
            else
            {
                // 停止中
                animator.SetBool("IsGrabbingIdle", true);
                animator.SetBool("IsPushing", false);
                animator.SetBool("IsPulling", false);
            }
        }
        else
        {
            // 掴んでいない時
            animator.SetBool("IsGrabbingIdle", false);
            animator.SetBool("IsPushing", false);
            animator.SetBool("IsPulling", false);
        }
    }

    // --- トリガー関連 (元々の機能を維持) ---
    private void OnTriggerEnter(Collider other)
    {
        // バブル処理
        if (other.CompareTag("Bubble"))
        {
            AudioSource hitAudio = other.GetComponent<AudioSource>();
            if (hitAudio) hitAudio.Play();
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<Collider>().enabled = false;
            Destroy(other.gameObject, hitAudio ? hitAudio.clip.length : 0f);
        }

        playercatch detector = other.GetComponent<playercatch>();
        if (detector != null) currentTargetDetector = detector;
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentTargetDetector != null && other.GetComponent<playercatch>() == currentTargetDetector)
            currentTargetDetector = null;
    }

    // 梯子用メソッド群
    public void Slope() { isClimbing = true; velocity = Vector3.zero; }
    public void ExitSlope() { isClimbing = false; }
    public void ExitSlopeTop(float groundY)
    {
        isClimbing = false;
        controller.enabled = false;
        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
        controller.enabled = true;
        StartCoroutine(DisableGravityAndPush());
    }
    private IEnumerator DisableGravityAndPush()
    {
        controller.Move(transform.forward * 0.5f);
        yield return new WaitForSeconds(0.2f);
    }
    public void ExitSlopeBottom(float groundY)
    {
        isClimbing = false;
        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
        velocity.y = -2f;
    }
}
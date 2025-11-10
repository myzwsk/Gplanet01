using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Input Systemから呼ばれる
    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }
    public void OnJump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        // 入力から移動ベクトルを作成
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * speed * Time.deltaTime);

        // 重力処理
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 地面に押し付ける
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

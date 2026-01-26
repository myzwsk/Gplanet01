using UnityEngine;
using UnityEngine.InputSystem;

public class BossCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 0.2f;   // マウス用感度（大きめに）
    public float gamepadSensitivity = 120f; // ゲームパッド用感度（角度/秒）
    public float distance = 5f;
    public float smoothSpeed = 10f;

    private Vector2 lookInputRaw; // 生の入力値（deltaTime を掛けない）
    private float yaw = 0f;
    private float pitch = 20f;

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            var device = context.control?.device;

            if (device is Mouse)
            {
                lookInputRaw = input * mouseSensitivity;
            }
            else if (device is Gamepad)
            {
                lookInputRaw = input * gamepadSensitivity;
            }
        }
        else if (context.canceled)
        {
            // 入力が止まったらゼロにする
            lookInputRaw = Vector2.zero;
        }
    }


    void LateUpdate()
    {
        // deltaTime をここで掛ける（フレームレート非依存）
        Vector2 look = lookInputRaw * Time.deltaTime;

        // 入力を回転に変換
        yaw += look.x;
        pitch -= look.y;

        // 下に行かないように制限
        pitch = Mathf.Clamp(pitch, 0f, 60f);

        // プレイヤーを中心にカメラ位置を更新
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = player.position + rotation * new Vector3(0, 0, -distance);

        // スムージング
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, smoothSpeed * Time.deltaTime);
    }
}

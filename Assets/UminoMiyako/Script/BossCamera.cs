using UnityEngine;
using UnityEngine.InputSystem;

public class BossCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 0.2f;   // マウス用感度（大きめに）
    public float gamepadSensitivity = 120f; // ゲームパッド用感度（角度/秒）
    public float distance = 5f;
    public float smoothSpeed = 10f;

    private Vector2 lookInput;
    private float yaw = 0f;
    private float pitch = 20f;

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // デバイスごとに処理を分ける
        if (context.control.device is Mouse)
        {
            // マウスはピクセル移動量なのでそのまま使う
            lookInput = input * mouseSensitivity;
        }
        else if (context.control.device is Gamepad)
        {
            // スティックは -1〜1 の値なので deltaTime を掛ける
            lookInput = input * gamepadSensitivity * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        // 入力を回転に変換
        yaw += lookInput.x;
        pitch -= lookInput.y;

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

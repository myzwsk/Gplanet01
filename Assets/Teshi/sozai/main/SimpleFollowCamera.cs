using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleFollowCamera : MonoBehaviour
{
    public Transform target;

    [Header("Position")]
    public Vector3 offset = new Vector3(0, 2, -4);
    public float smooth = 8f;

    [Header("Look Sensitivity")]
    public float mouseSensitivity = 2.5f;        // マウス感度
    public float controllerSensitivity = 120f;   // コントローラー感度（右スティック）

    [Header("Pitch Limit")]
    public float minPitch = -30f;
    public float maxPitch = 60f;

    public bool follow = true;

    // 固定カメラ
    public bool isFixedCamera = false;
    public Transform fixedPoint;

    float yaw;
    float pitch;

    // 新 Input System の Look 入力
    Vector2 lookInput;

    // 今の入力がゲームパッドかどうか
    bool isGamepad = false;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!follow || target == null) return;

        // 固定カメラ
        if (isFixedCamera && fixedPoint != null)
        {
            transform.position = fixedPoint.position;
            transform.rotation = fixedPoint.rotation;
            return;
        }

        // 入力値
        float inputX = lookInput.x;
        float inputY = lookInput.y;

        // ★ デバイスごとに感度を切り替え
        float sensitivity = isGamepad ? controllerSensitivity : mouseSensitivity;

        yaw += inputX * sensitivity * Time.deltaTime;
        pitch -= inputY * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPos = target.position + rotation * offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smooth * Time.deltaTime
        );

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    // PlayerInput → UnityEvent で呼ばれる
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();

        // ★ どのデバイスからの入力か判定
        isGamepad = context.control.device is Gamepad;
    }

    // 固定カメラ切り替え
    public void EnableFixedCamera()
    {
        isFixedCamera = true;
    }

    public void DisableFixedCamera()
    {
        isFixedCamera = false;
    }
}
